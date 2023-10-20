using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class G80_UIManager : MonoBehaviour
{
    public static G80_UIManager Instance;
    
    
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text bestScoreTxt;
    [SerializeField] private GameObject comboTxtPrefab;
    
    public void Awake()
    {
        SetSingleton();
       
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        bestScoreTxt.text = $"Best:{G80_GameManager.Instance.bestScore}";
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
}
