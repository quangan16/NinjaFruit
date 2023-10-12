using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SpawnMode CurrentMode { get; set; }

    private float Point { get; set;  } = 0;

    [SerializeField] private SpawnModeSO spawnMode;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeMode(SpawnMode targetMode)
    {
        CurrentMode = targetMode;
    }
    
    
}
