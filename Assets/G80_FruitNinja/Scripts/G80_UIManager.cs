using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class G80_UIManager : MonoBehaviour
{
    public static G80_UIManager Instance;
    
    
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text bestScoreTxt;
    [SerializeField] private Text readyTxt;
    [SerializeField] private Text sliceTxt;
    [SerializeField] private GameObject comboTxtPrefab;
    [SerializeField] private float initTextDuration;
    
    public void Awake()
    {
        SetSingleton();
       
    }

    public void Start()
    {
        Init();
        ShowInitText();
    }

    public void Init()
    {
        bestScoreTxt.text = $"Best:{G80_GameManager.Instance.bestScore}";
        ShowInitText();
    }

    public void SetSingleton()
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

    public void UpdatePoint()
    {
        scoreTxt.text = G80_GameManager.Instance.GetCurrentPoint().ToString();
    }

    public void ShowComboText(int combos)
    {
        G80_GameManager.Instance.check = true;
        // Vector3 comboDisPlayOffset = new Vector3(15.0f, 0.0f, 0.0f);
        Vector3 comboTxtPos = G80_GameManager.Instance.GetLastFruitComboPos();
        Instantiate(comboTxtPrefab, comboTxtPos, Quaternion.identity, gameObject.transform);
    }

    public void PopTextZoomInOut(Text text, float duration,  Action action, AudioClip audioClip,  float zoomInScale = 1.0f, float zoomOutScale = 0.0f)
    {
        G80_SoundManager.Instance.PlaySoundDelay(audioClip, duration);
        text.transform.DOScale(zoomInScale, duration ).SetEase(Ease.OutQuint).SetDelay(duration).OnComplete(() =>
        {
           
            text.transform.DOScale(zoomOutScale, duration).SetDelay(duration).OnComplete(() =>
            {
                action?.Invoke();
            });

        });
    }

    public void ShowInitText()
    {
        PopTextZoomInOut(readyTxt, initTextDuration, () =>
        {
            PopTextZoomInOut(sliceTxt, initTextDuration, null, G80_SoundManager.Instance.sfx.startClip);
        }, G80_SoundManager.Instance.sfx.readyClip);

        G80_GameManager.Instance.Invoke("StartGame", initTextDuration * 2);
      
      

    }
}
