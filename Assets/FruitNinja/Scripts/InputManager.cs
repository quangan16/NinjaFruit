using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private Vector3 lastFrameTouchPosition;
    private Vector3 currentTouchPosition;
    [SerializeField] private Vector3 mouseDirection;


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
    }

    public void UpdateTouch()
    {
        currentTouchPosition = Input.mousePosition;
        mouseDirection = mouseDirection = currentTouchPosition - lastFrameTouchPosition;
        lastFrameTouchPosition = currentTouchPosition;
    }

    public Vector3 GetMouseDirectionNomalized()
    {
        return mouseDirection.normalized;
    }

    public Vector3 GetTouchPosition()
    {
        return currentTouchPosition;
    }
    
    
}
