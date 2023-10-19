using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SuperFruit : MonoBehaviour
{
    [SerializeField] private TextMeshPro superComboTxt;
    [SerializeField] private int superCombo;
    [SerializeField] private Rigidbody fruitRb;
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject wholeFruit;
    private bool firstTouch = false;
    [SerializeField] private GameObject slicedFruit;
    [SerializeField] private ParticleSystem juiceParticle;
    [SerializeField] private Vector2 randomCameraTargetPos = Vector2.zero;
    [SerializeField] private float circleRadius = 0.01f;
    
    public void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public void Update()
    {
        
        UpdateComboText();
        UpdateRandomPosInsideCircle();
        Debug.Log(Vector2.Distance(GameManager.Instance.CurrentCamPos, transform.position));
        
    }
    // Start is called before the first frame update
    public void OnMouseEnter()
    {
        Vector3 touchPosition = InputManager.Instance.GetTouchPosition();
        Vector3 touchDirection = InputManager.Instance.GetTouchDirectionNomalized();
        if (InputManager.Instance.GetSwipeVelocity() >= InputManager.Instance.minSwipeVelocity)
        {
            OnSliced(touchDirection, touchPosition, 6.0f);
            
            
        }
       
    }


    public void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
    {
        GameManager.Instance.AddPoint(1);
        UIManager.Instance.UpdatePoint();
        superCombo++;
        if (firstTouch == false)
        {
            ShowComboText();
            StartCoroutine(OnFirstSliced());
        }
        else
        {
            MoveCamTowardTarget();
        }
       
    }

    public void ShowComboText()
    {
        if (superComboTxt.IsActive() == false)
        {
            superComboTxt.gameObject.SetActive(true);
        }
        
    }

    public void UpdateComboText()
    {
        superComboTxt.text = $"Ultra combo\nx{superCombo}";
    }

    public IEnumerator OnFirstSliced()
    {
       
        firstTouch = true;
        float randomAngle = Random.Range(0f, 360.0f);
        randomCameraTargetPos = (Vector2)transform.position +
                                new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * circleRadius;
       StartCoroutine(GameManager.Instance.ZoomInCamera(15.0f, gameObject, 0.3f));
       StartCoroutine(GameManager.Instance.EnterSlowMotion(0.3f));
      
      

        yield return new WaitForSecondsRealtime(5.0f);
       StartCoroutine(GameManager.Instance.ResetCamera());
        StartCoroutine(GameManager.Instance.ExitSlowMotion());
       
     
      
        yield break;
    }

    public void UpdateRandomPosInsideCircle()
    {
       
        if (Vector2.Distance(GameManager.Instance.CurrentCamPos, transform.position) >
            circleRadius)
        {
           
            float randomAngle = Random.Range(0f, 360.0f);
            randomCameraTargetPos =(Vector2)transform.position +  new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * circleRadius;
        }
    }

    public void MoveCamTowardTarget()
    {
        
        Camera.main.transform.DOMove(Camera.main.transform.position +  new Vector3(randomCameraTargetPos.x , randomCameraTargetPos.y ,
            0) * 0.4f , 0.05f).SetEase(Ease.OutBack);
    }
    
    
}
