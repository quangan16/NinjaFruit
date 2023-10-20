using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class G80_ComboText : MonoBehaviour
{
    public Text combo;
    public void Awake()
    {
        string comboInt = G80_GameManager.Instance.combo.ToString();
        combo.text = $"COMBO\nx{comboInt}\n+{comboInt} bonus pts";
    }
    public void OnEnable()
    {
        ShowText();
        HideText();
    }

    public void OnDisable()
    {
        transform.DOKill();
    }

    public void OnInit()
    {
        
    }
    public void OnDestroy()
    {
        transform.DOKill();
    }

    public void ShowText()
    {
        transform.DOScale(Vector3.one * 5.0f, 0.6f).SetEase(Ease.OutBack);
    }

    public void HideText()
    {
        transform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InBack).SetDelay(2.0f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    
    
    
    
}
