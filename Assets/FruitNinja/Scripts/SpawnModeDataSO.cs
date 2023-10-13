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


    public float startDelay = 1.5f;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 1.0f;

    public int maxBurstFruits = 0;
    public float burstInterval = 2.0f;

}

public enum SpawnMode
{
    WARMUP,
    SLOW,
    FAST,
    BURST_2,
    BURST_3,

}