using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnModeData", menuName = "SO/SpawnMode", order = 1)]
public class G80_SpawnModeDataSO : ScriptableObject
{
    public SpawnModeData[] spawnModeData;
}

[Serializable]
public class SpawnModeData
{
    [Header("Spawner properties")] 
    public SpawnMode spawnMode;
    public GameObject[] fruitPrefabs;

    public float minForce = 60.0f;
    public float maxForce = 100.0f;
    public float minSideForce = 80.0f;
    public float maxSideForce = 120.0f;
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
    BURST_2,
    FAST,
    BURST_3,
    BURST_4,
    CRAZY,
    BOSS

}