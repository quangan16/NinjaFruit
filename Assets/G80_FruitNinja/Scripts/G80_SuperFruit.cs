using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class G80_SuperFruit : MonoBehaviour
{
    [SerializeField] private TextMeshPro superComboTxt;
    [SerializeField] private int superCombo;
    [SerializeField] private Rigidbody fruitRb;
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject wholeFruit;
    private bool firstTouch = false;
    [SerializeField] private GameObject slicedFruit;
    [SerializeField] private ParticleSystem juiceParticle;
    [SerializeField] private Transform previousPos;
    [SerializeField] private Transform randomCameraTargetPos;
    [SerializeField] private Transform currentCameraPos;

    [SerializeField] private float circleRadius = 0.01f;
    [SerializeField] private float zoomMagnitude;
    [SerializeField] private bool followTarget;
    [SerializeField] private bool updated = false;
    [SerializeField] private Vector2 cameraOffset;
    [SerializeField] private float slowDuration;
    [SerializeField] private float slowRate;
    public void Awake()
    {   
        zoomMagnitude = (collider as SphereCollider).radius / 0.001f;
    }

    public void Update()
    {
        UpdateRandomPosInsideCircle();
        CameraFollowTarget(currentCameraPos.position);
        UpdateComboText();
        
    }
    // Start is called before the first frame update
    public void OnMouseEnter()
    {
        Vector3 touchPosition = G80_InputManager.Instance.GetTouchPosition();
        Vector3 touchDirection = G80_InputManager.Instance.GetTouchDirectionNomalized();
        if (G80_InputManager.Instance.GetSwipeVelocity() >= G80_InputManager.Instance.minSwipeVelocity)
        {
            OnSliced(touchDirection, touchPosition, 6.0f);
            
            
        }
       
    }


    public void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
    {
        G80_GameManager.Instance.AddPoint(1);
        G80_UIManager.Instance.UpdatePoint();
        superCombo++;
        if (firstTouch == false)
        {
            ShowComboText();
            StartCoroutine(OnFirstSliced());
        }
        else
        {
            if (updated == true) updated = false;
            MoveCamTowardTarget();
        }
       
    }

    public void CameraFollowTarget(Vector2 targetPos)
    {
        if (followTarget)
        {
            G80_GameManager.Instance.MakeCamFollowTarget(targetPos);
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
        followTarget = true;
        DOVirtual.DelayedCall(slowDuration, () => { followTarget = false; });
        float randomAngle = Random.Range(0f, 360.0f);
        randomCameraTargetPos.position = (Vector2)transform.localPosition +
                                new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * circleRadius;
        cameraOffset = (randomCameraTargetPos.position - currentCameraPos.position) * 0.25f;
        StartCoroutine(G80_GameManager.Instance.EnterSlowMotion(slowDuration, slowRate));
        StartCoroutine(G80_GameManager.Instance.ZoomInCamera(zoomMagnitude, gameObject, 0.3f, slowDuration));
        
       // StartCoroutine(G80_GameManager.Instance.ResetCamera());
     
       
     
      
        yield break;
    }
    
    public void UpdateRandomPosInsideCircle()
    {
       
        if (Vector2.Distance(currentCameraPos.position, transform.position) > circleRadius && updated == false  && firstTouch)
        {
            previousPos.position = randomCameraTargetPos.position;
            float randomAngle = Random.Range(0f, 360.0f);
            randomCameraTargetPos.position =(Vector2)transform.position +  new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * circleRadius;
            updated = true;
            cameraOffset = (randomCameraTargetPos.position - currentCameraPos.position) * 0.25f;
            
        }

        
    }

    public void MoveCamTowardTarget()
    {
        
        // currentCameraPos.DOLocalMove( currentCameraPos .position + new Vector3(randomCameraTargetPos.position.x , randomCameraTargetPos.position.y ,
        //     0) , 0.05f).SetEase(Ease.OutBack);
        currentCameraPos.DOMove((Vector2)currentCameraPos.position + cameraOffset, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(randomCameraTargetPos.position, 20.0f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(previousPos.position, 15.0f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(currentCameraPos.position, 25.0f);
    }
}
