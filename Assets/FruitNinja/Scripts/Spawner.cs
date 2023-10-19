using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public abstract class Spawner : MonoBehaviour
{
   
    protected BoxCollider spawnArea;
    [SerializeField] protected GameObject[] fruitPrefabs;


    [SerializeField] protected float startDelay = 1.5f;
    [SerializeField] protected float minSpawnInterval = 0.5f;
    [SerializeField] protected float maxSpawnInterval = 1.0f;

    [Space(10)]
    [SerializeField] protected float minForce = 500.0f;
    [SerializeField] protected float maxForce = 1000.0f;

    [Space(10)] 
    [Header("Fruit properties")] 
    [SerializeField]protected float minInitAngle = 5.0f;
    [SerializeField] protected float maxInitAngle = 15.0f;
    [SerializeField] protected float fruitLifetime = 5.0f;

    [SerializeField] protected int maxBurstFruits = 0;
    [SerializeField] protected float burstInterval = 0;

    [SerializeField] protected int currentBusrtFruits = 1;
    [SerializeField] protected float randomTimeToNextBomb = 0;
    [SerializeField] protected bool bombAvailable = false;
    
   
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

    public abstract IEnumerator Spawn();
    
    public void OnModeChange(SpawnMode mode)
    {
        StopAllCoroutines();
        UpdateData();
        StartCoroutine(Spawn());
    }

    public virtual void UpdateData()
    {
        fruitPrefabs = GameManager.Instance.GetCurrentModeData().fruitPrefabs;
        startDelay = GameManager.Instance.GetCurrentModeData().startDelay;
        minForce = GameManager.Instance.GetCurrentModeData().minForce;
        maxForce = GameManager.Instance.GetCurrentModeData().maxForce;
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

   
    public IEnumerator UpdateBombTime()
    {
        float timeToNextBomb = Random.Range(randomTimeToNextBomb - 5.0f, randomTimeToNextBomb + 5.0f);
        while (timeToNextBomb > 0.0f)
        {
            timeToNextBomb -= Time.unscaledDeltaTime;
            yield return null;
        }

        bombAvailable = true;
    }

    public abstract void SpawnBomb();



}


