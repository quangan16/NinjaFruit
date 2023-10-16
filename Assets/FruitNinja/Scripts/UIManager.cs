using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    
    [SerializeField] private Text pointTxt;
    [SerializeField] private Text comboTxt;
    
    public void Awake()
    {
        SetSingleton();
       
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
        pointTxt.text = GameManager.Instance.GetCurrentPoint().ToString();
    }

    public void ShowComboText(int combos)
    {
        Vector3 comboDisPlayOffset = new Vector3(15.0f, 0.0f, 0.0f);
        comboTxt.transform.position = InputManager.Instance.GetTouchPosition() + comboDisPlayOffset;
        comboTxt.text = "Combo x" + combos;
    }
}
