using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    
    [SerializeField] private Text pointTxt;

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
}
