using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public class Spawner : MonoBehaviour
{
   
    private BoxCollider spawnArea;
    [SerializeField] private GameObject[] fruitPrefabs;


    [SerializeField] private float startDelay = 1.5f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 1.0f;

    [Space(10)]
    [SerializeField] private float minForce = 500.0f;
    [SerializeField] private float maxForce = 1000.0f;

    [Space(10)] 
    [Header("Fruit properties")] 
    [SerializeField]private float minInitAngle = 15.0f;
    [SerializeField] private float maxInitAngle = 25.0f;
    [SerializeField] private float fruitLifetime = 5.0f;

    [SerializeField] private int maxBurstFruits = 0;
    [SerializeField] private float burstInterval = 0;

    [SerializeField] private int currentBusrtFruits = 1;
 

   
    public void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
        
    }

   
    
    public void Start()
    {
        GameManager.Instance.OnModeChanged += OnModeChange;
        UpdateData();
        StartCoroutine(Spawn());
        

    }

    public void OnDisable()
    {
        GameManager.Instance.OnModeChanged -= OnModeChange;
        StopAllCoroutines();
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startDelay);
        while (enabled)
        {
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            var spawnPosX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            var spawnPosY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            var spawnPosZ = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            var intervalTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            var shootForce = Random.Range(minForce, maxForce);
            var randomAngleOffset = Random.Range(minInitAngle, maxInitAngle);
            var initAngle = Quaternion.Euler(fruitPrefab.transform.rotation.eulerAngles.x,
                fruitPrefab.transform.rotation.eulerAngles.y, CalculateAngles(spawnPosX, randomAngleOffset));
            // var initAngle = CalculateAngles(spawnPosX);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            if (GameManager.Instance.CurrentMode > SpawnMode.FAST)
            {
                if (currentBusrtFruits < maxBurstFruits)
                {
                    GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                    newFruit.GetComponent<Rigidbody>().AddForce(Vector3.up * shootForce, ForceMode.Impulse);
                    newFruit.GetComponent<Rigidbody>().AddTorque(newFruit.transform.forward * initAngle.z*  shootForce * 5, ForceMode.Impulse);
                    Destroy(newFruit, fruitLifetime);
                    yield return new WaitForSeconds(burstInterval);
                    currentBusrtFruits++;
                }
                else
                {
                    yield return new WaitForSeconds(intervalTime);
                    currentBusrtFruits = 0;
                }
                
            }
            else
            {
                GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
                newFruit.GetComponent<Rigidbody>().AddForce(newFruit.transform.up * shootForce, ForceMode.Impulse);
                newFruit.GetComponent<Rigidbody>()
                    .AddTorque(newFruit.transform.forward * initAngle.z* shootForce * 5, ForceMode.Impulse);
                Destroy(newFruit, fruitLifetime);
                yield return new WaitForSeconds(intervalTime);
            }
         
          
           
        }
        yield  break;
    }

    public void OnModeChange(SpawnMode mode)
    {
        StopAllCoroutines();
        UpdateData();
        StartCoroutine(Spawn());
    }

    public void UpdateData()
    {
        fruitPrefabs = GameManager.Instance.GetCurrentModeData().fruitPrefabs;
        startDelay = GameManager.Instance.GetCurrentModeData().startDelay;
        minSpawnInterval = GameManager.Instance.GetCurrentModeData().minSpawnInterval;
        maxSpawnInterval = GameManager.Instance.GetCurrentModeData().maxSpawnInterval;
        maxBurstFruits = GameManager.Instance.GetCurrentModeData().maxBurstFruits;
        burstInterval = GameManager.Instance.GetCurrentModeData().burstInterval;
    }

   
    public float CalculateAngles(float spawnPosX, float randomOffset)
    {
        float angle;
        if (spawnPosX < 0)
        {
            angle = Random.Range(0.0f, spawnPosX - randomOffset);
        }
        else if (spawnPosX > 0)
        {
            angle = Random.Range(0.0f, spawnPosX + randomOffset);
        }
        else
        {
            angle = Random.Range(-randomOffset, randomOffset);
        }


        return angle;
    }

    
    
}


