using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    public UnityAction<SpawnMode> OnModeChanged;
    public static GameManager Instance;
    public SpawnMode CurrentMode { get; set; }
    

    public float Point  = 0;

    public int[] ModeScripts;
    public int currentScript;

    public SpawnModeDataSO modeDataSO;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        MakeSingleton();
        ModeScripts = new int[8] { 0, 1, 2, 3, 4, 5, 5, 5, };
        CurrentMode = SpawnMode.WARMUP;
    }

    public void MakeSingleton()
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
        OnModeChanged?.Invoke(targetMode);
    }


    public void Update()
    {
        UpdateModeByPoint();
    }

    public void UpdateModeByPoint()
    {
        currentScript = 0;
        if (currentScript / ModeScripts.Length - 1 <= 1)
        {
            if (Point % 10 == 0)
            {
                ChangeMode((SpawnMode)ModeScripts[++currentScript]);
            }

            else
            {
                currentScript = 2;
            }


        }

    }

    public SpawnModeData GetCurrentModeData()
    {
        return modeDataSO.spawnModeData[(int)CurrentMode];
    }
}
