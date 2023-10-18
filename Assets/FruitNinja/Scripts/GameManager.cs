using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

   
    public static GameManager Instance;
    public SpawnMode CurrentMode;
    public event Action<SpawnMode> OnModeChanged;

    public int Point  = 0;

    public int[] ModeScripts;
    public int currentScript = 0;

    public SpawnModeDataSO modeDataSO;

    public bool isCutting;

    public int combo;
    public float comboCountdown = 1.0f;
    public Vector3 lastFruitHitPos;
    public bool check = false;
    public void Awake()
    {
        Init();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Physics.gravity = new Vector3(0.0f, -35.0f, 0.0f);
    }

    public void OnDestroy()
    {
        Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);
    }

    public int targetPoint = 0;

    public void Init()
    {
        MakeSingleton();
        ModeScripts = new int[8] { 0, 1, 2, 3, 4, 5, 5, 5, };
        CurrentMode = SpawnMode.WARMUP;
        targetPoint += modeDataSO.spawnModeData[(int)CurrentMode].pointToPass;
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
            // Debug.Log($"Mode changed to {targetMode}");
        }
       
    }


    public void Update()
    {
        UpdateModeByPoint();
    }

    public void UpdateModeByPoint()
    {
        if (Point <= 90)
        {
            if (Point >= targetPoint)
            {
                targetPoint = Point + modeDataSO.spawnModeData[(int)CurrentMode].pointToPass;
                currentScript++;
            }
        }
     

      else
      {
          currentScript = (int)SpawnMode.CRAZY;
      }

      ChangeMode((SpawnMode)ModeScripts[currentScript]);
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

    public IEnumerator ShowCombo()
    {


        while (comboCountdown > 0)
        {
            comboCountdown -= Time.deltaTime;
            yield return null;
        }

        if (combo >= 3)
        {
            UIManager.Instance.ShowComboText(combo);
            
        }
       
        combo = 0;
        comboCountdown = 1.0f;
        check = false;
        yield break;
    }
    

    public Vector3 GetLastFruitComboPos()
    {
        return lastFruitHitPos;
    }


}
