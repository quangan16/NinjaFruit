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

    [Space(10)] [SerializeField] private float minForce = 50.0f;
    [SerializeField] private float maxForce = 100.0f;

    [Header("Fruit properties")] [Space(10)] [SerializeField]
    private float minInitAngle = 15.0f;

    [SerializeField] private float maxInitAngle = 25.0f;
    [SerializeField] private float fruitLifetime = 5.0f;

 

   
    public void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
        UpdateData();
    }
    
    public void OnEnable()
    {
        GameManager.Instance.OnModeChanged += OnModeChange;
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
            var initAngle = Quaternion.Euler(0.0f, 0.0f, Random.Range(minInitAngle, maxInitAngle));
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, spawnPosZ);
            GameObject newFruit = Instantiate(fruitPrefab, spawnPos, initAngle);
            Destroy(newFruit, fruitLifetime);
            yield return new WaitForSeconds(intervalTime);
        }
        yield  break;
    }

    public void OnModeChange(SpawnMode mode)
    {
        // StopAllCoroutines();
        UpdateData();
    }

    public void UpdateData()
    {
        fruitPrefabs = GameManager.Instance.GetCurrentModeData().fruitPrefabs;
        startDelay = GameManager.Instance.GetCurrentModeData().startDelay;
        minSpawnInterval = GameManager.Instance.GetCurrentModeData().minSpawnInterval;
        maxSpawnInterval = GameManager.Instance.GetCurrentModeData().maxSpawnInterval;
      
    }

    
    
}


