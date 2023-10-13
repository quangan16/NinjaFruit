using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

   
    public static GameManager Instance;
    public SpawnMode CurrentMode;
    public event Action<SpawnMode> OnModeChanged;

    public int Point  = 0;

    public int[] ModeScripts;
    public int currentScript = 0;

    public SpawnModeDataSO modeDataSO;

    public void Awake()
    {
        Init();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void Init()
    {
        MakeSingleton();
        ModeScripts = new int[8] { 0, 5, 2, 3, 4, 5, 5, 5, };
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
        if (CurrentMode != targetMode)
        {
            CurrentMode = targetMode;
            OnModeChanged?.Invoke(targetMode);
            Debug.Log($"Mode changed to {targetMode}");
        }
       
    }


    public void Update()
    {
        UpdateModeByPoint();
    }

    public void UpdateModeByPoint()
    {
      
        if (currentScript / ModeScripts.Length - 1 < 1)
        {
            if (Point > 0 && Point / 10 > currentScript )
            {
                ChangeMode((SpawnMode)ModeScripts[++currentScript]);
            }
            
        }
        else
        {
            currentScript = 2;
        }

    }

    public SpawnModeData GetCurrentModeData()
    {
        return modeDataSO.spawnModeData[(int)CurrentMode];
    }

    public void AddPoint(int point)
    {
        Point += point;
    }

    public int GetCurrentPoint()
    {
        return Point;
    }
}
