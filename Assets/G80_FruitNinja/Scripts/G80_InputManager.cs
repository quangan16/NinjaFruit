using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G80_InputManager : MonoBehaviour
{
    public static G80_InputManager Instance;
    private Vector3 lastFrameTouchPosition;
    private Vector3 currentTouchPosition;
    [SerializeField] private Vector3 mouseDirection;
    public double swipeVelocity = 0.0f;
    public float minSwipeVelocity = 0.05f;


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

    private void Awake()
    {
        MakeSingleton();
    }
    private void Start()
    {
       
        lastFrameTouchPosition = Input.mousePosition;
        currentTouchPosition = Input.mousePosition;
        
        mouseDirection = Vector3.zero;
        
    }

    private void Update()
    {
        UpdateTouch();
        // Debug.Log(InputManager.Instance.GetSwipeVelocity());
    }
    
    public void UpdateTouch()
    {
        currentTouchPosition = Input.mousePosition;
        mouseDirection  = currentTouchPosition - lastFrameTouchPosition;
        swipeVelocity = mouseDirection.magnitude / Time.deltaTime;
        lastFrameTouchPosition = currentTouchPosition;
       
    }

    public Vector3 GetTouchDirectionNomalized()
    {
        return mouseDirection.normalized;
    }

    public Vector3 GetTouchPosition()
    {
        return currentTouchPosition;
    }

    public double GetSwipeVelocity()
    {
        return swipeVelocity;
    }
    
    
}
