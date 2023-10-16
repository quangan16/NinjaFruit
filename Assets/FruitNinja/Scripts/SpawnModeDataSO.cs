using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnModeData", menuName = "SO/SpawnMode", order = 1)]
public class SpawnModeDataSO : ScriptableObject
{
    public SpawnModeData[] spawnModeData;
}

[Serializable]
public class SpawnModeData
{
    [Header("Spawner properties")] 
    public SpawnMode spawnMode;
    public GameObject[] fruitPrefabs;


    [SerializeField] private float minForce = 500.0f;
    [SerializeField] private float maxForce = 1000.0f;
    public float startDelay = 1.5f;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 1.0f;

    public int maxBurstFruits = 0;
    public float burstInterval = 2.0f;
    public int pointToPass = 5;

}

public enum SpawnMode
{
    WARMUP,
    SLOW,
    FAST,
    BURST_2,
    BURST_3,
    BURST_4,
    CRAZY

}