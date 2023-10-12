using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnModeData", menuName = "SO/SpawnMode", order = 1)]
public class SpawnModeSO : ScriptableObject
{
    public SpawnModeData[] spawnModes;
}

[Serializable]
public class SpawnModeData
{
    [Header("Spawner properties")] 
    [SerializeField] private SpawnMode spawnMode;
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
}

public enum SpawnMode
{
    WARMUP,
    SLOW,
    FAST,
    BURST_2,
    BURST_3,

}