using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;


[Serializable]
public abstract class G80_Spawner : MonoBehaviour
{
   
    protected BoxCollider spawnArea;
    [SerializeField] protected GameObject[] fruitPrefabs;
    [SerializeField] protected GameObject bombPrefabs;


    [SerializeField] protected float startDelay = 1.5f;
    [SerializeField] protected float minSpawnInterval = 0.5f;
    [SerializeField] protected float maxSpawnInterval = 1.0f;

    [Space(10)]
    [SerializeField] protected float minForce = 500.0f;
    [SerializeField] protected float maxForce = 1000.0f;

    [Space(10)] 
    [Header("Fruit properties")] 
    [SerializeField]protected float minInitAngle = 0.0f;
    [SerializeField] protected float maxInitAngle = 0.0f;
    [SerializeField] protected float fruitLifetime = 5.0f;

    [SerializeField] protected int maxBurstFruits = 0;
    [SerializeField] protected float burstInterval = 0;

    [SerializeField] protected int currentBusrtFruits = 1;
    [SerializeField] protected float randomTimeToNextBomb = 25;
    [SerializeField] protected bool bombAvailable = true;
    
   
    public void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
        
    }

    
    public virtual void Start()
    {

        UpdateData();
        G80_GameManager.Instance.OnModeChanged += OnModeChange;
     

    }

    public void OnEnable()
    {
        
        G80_GameManager.onGameStart += onStart;

        void onStart()
        {
            StartCoroutine(Spawn());
        }
    }

    public void OnDisable()
    {
        G80_GameManager.Instance.OnModeChanged -= OnModeChange;
        StopAllCoroutines();
    }


       
   

    public abstract IEnumerator Spawn();
    
    public virtual void OnModeChange(SpawnMode mode)
    {
        StopAllCoroutines();
        UpdateData();
        StartCoroutine(Spawn());
      
    }

    public virtual void UpdateData()
    {
        fruitPrefabs = G80_GameManager.Instance.GetCurrentModeData().fruitPrefabs;
        startDelay = G80_GameManager.Instance.GetCurrentModeData().startDelay;
        minForce = G80_GameManager.Instance.GetCurrentModeData().minForce;
        maxForce = G80_GameManager.Instance.GetCurrentModeData().maxForce;
        minSpawnInterval = G80_GameManager.Instance.GetCurrentModeData().minSpawnInterval;
        maxSpawnInterval = G80_GameManager.Instance.GetCurrentModeData().maxSpawnInterval;
        maxBurstFruits = G80_GameManager.Instance.GetCurrentModeData().maxBurstFruits;
        burstInterval = G80_GameManager.Instance.GetCurrentModeData().burstInterval;
    }
    
   
    public float CalculateSpawnAngles(float spawnPosX, float randomOffset)
    {
        float angle;
        if (spawnPosX < spawnArea.bounds.min.x * 0.3)
        {
            angle = Random.Range(0.0f, 0 - randomOffset);
        }
        else if (spawnPosX > spawnArea.bounds.max.x * 0.3)
        {
            angle = Random.Range(0.0f, 0 + randomOffset);
        }
        else
        {
            angle = Random.Range(-randomOffset, randomOffset);
        }


        return angle;
    }

   
   

    public abstract IEnumerator SpawnBomb();



}


