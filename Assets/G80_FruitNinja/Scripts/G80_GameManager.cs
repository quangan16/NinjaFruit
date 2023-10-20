using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FruitNinja.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class G80_GameManager : MonoBehaviour
{


    public static G80_GameManager Instance;
    public SpawnMode CurrentMode;
    public event Action<SpawnMode> OnModeChanged;
    private float defaultCameraOrthoSize = 30.0f;
    public int score = 0;
    public int progressScore;
    public int bestScore = 0;
    public int[] ModeScripts;
    public int currentScript = 0;

    public G80_SpawnModeDataSO modeDataSO;
    public Camera cam;
    public bool isCutting;
    public float superFruitTime = 4;
    public int combo;
    public float comboCountdown = 1.0f;
    public Vector3 lastFruitHitPos;
    public bool check = false;
    public GameObject targetObject;
    public bool inSlowMotion = false;
    public bool isZoomIn = false;
    public List<GameObject> activeFruits;
  

    public Vector3 CurrentCamPos
    {
        get
        {
            return cam.transform.position;
        }
        set
        {
            cam.transform.position = value;
        }
        
    }

    public void Awake()
    {
        Init();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Physics.gravity = new Vector3(0.0f, -100.0f, 0.0f);
    }

    public void OnDestroy()
    {
        Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);
    }

    public int targetPoint = 0;

    public void Init()
    {
        MakeSingleton();
        cam = Camera.main;
        ModeScripts = new int[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8, };
        CurrentMode = SpawnMode.WARMUP;
        bestScore = G80_DataManager.Instance.LoadBestScore();
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

    public void UpdataBestScore()
    {
        if (score > bestScore)
        {
            G80_DataManager.Instance.SaveBestScore(score);
        }
    }
    
    public void UpdateModeByPoint()
    {
        if (progressScore <= 150)
        {
            if (progressScore >= targetPoint)
            {
                targetPoint = progressScore + modeDataSO.spawnModeData[(int)CurrentMode].pointToPass;
                currentScript++;
            }
        }


        else
        {
            currentScript = (int)SpawnMode.WARMUP;
        }

        ChangeMode((SpawnMode)ModeScripts[currentScript]);
    }

    public SpawnModeData GetCurrentModeData()
    {
        return modeDataSO.spawnModeData[(int)CurrentMode];
    }

    public void AddPoint(int point)
    {
        progressScore += point;
        score += point;
    }

    public void MinusPoint(int point)
    {
        score -= point;
    }

    public void ResetPoint()
    {
        score = 0;
    }

    public int GetCurrentPoint()
    {
        return score;
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
            comboCountdown += 0.5f;
            G80_UIManager.Instance.ShowComboText(combo);
            G80_GameManager.Instance.AddPoint(combo);
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


    public IEnumerator EnterSlowMotion(float targetTimeScale)
    {
        if (inSlowMotion == false)
        {
            inSlowMotion = true;
            float elapsedTime = 0.0f;
            float transitionTime = 0.6f;
            float originTimeScale = Time.timeScale;
            Debug.Log("Slow");
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(originTimeScale, targetTimeScale, elapsedTime / transitionTime);
                yield return null;
            }

            Time.timeScale = targetTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
       
    }

    public IEnumerator ExitSlowMotion()
    {
        if (inSlowMotion)
        {
            inSlowMotion = false;
            float elapsedTime = 0.0f;
            float transitionTime = 0.6f;
            float originTimeScale = Time.timeScale;
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(originTimeScale, 1.0f, elapsedTime / transitionTime);
                yield return null;
            }

            Time.timeScale = 1.0f;
        }
      
    }

    public IEnumerator ZoomInCamera(float targetZoomInSize, GameObject targetObject, float transitionTime)
    {
        if (isZoomIn == false)
        {
             float originOrthoSize = Camera.main.orthographicSize;
            Vector3 originPosition = Camera.main.transform.position;
            Vector3 targetPostion = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y,
                cam.transform.position.z);
            float elapsedTime = 0.0f;
            while (elapsedTime < transitionTime)
            {
              cam.orthographicSize = Mathf.Lerp(originOrthoSize, targetZoomInSize, elapsedTime/transitionTime);
              cam.transform.position = Vector3.Lerp(originPosition, targetPostion, elapsedTime / transitionTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            cam.orthographicSize = targetZoomInSize;
            cam.transform.position = targetPostion;
            isZoomIn = true;
        }
       
    }

    public void ShakeCamara()
    {
        cam.DOShakePosition(1.0f, 2.0f, 10, 90.0f, true).OnComplete(()=>StartCoroutine( ResetCamera()));
    }

    public IEnumerator ResetCamera()
    {
        if (isZoomIn)
        {
            isZoomIn = false;
            float originOrthoSize = Camera.main.orthographicSize;
            Vector3 originPosition = Camera.main.transform.position;
            Vector3 targetPostion = new Vector3(0, 0, -200.0f);
            float elapsedTime = 0.0f;
            float transitionTime =0.3f;
            while (elapsedTime < transitionTime)
            {
                Camera.main.orthographicSize =
                    Mathf.Lerp(originOrthoSize, defaultCameraOrthoSize, elapsedTime / transitionTime);
                Camera.main.transform.position =
                    Vector3.Lerp(originPosition, targetPostion, elapsedTime / transitionTime);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            Camera.main.orthographicSize = defaultCameraOrthoSize;
            Camera.main.transform.position = targetPostion;
        }

       
    }

    public void AddObjectToActiveList(GameObject obj)
    {
        activeFruits.Add(obj);
    }

    public void DestroyAllActiveFruits()
    {
        foreach (var fruit in activeFruits)
        {
            Destroy(fruit.gameObject);
        }
    }

    public void OnGameFinish()
    {
        UpdataBestScore();
    }

}
