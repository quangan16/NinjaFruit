using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    
    [SerializeField] private Rigidbody fruitRb;
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject wholeFruit;

    [SerializeField] private GameObject slicedFruit;
    
    public void Awake()
    {
        collider = GetComponent<Collider>();
    }


    public void OnMouseOver()
    {
        Vector3 touchPosition = InputManager.Instance.GetTouchPosition();
        Vector3 touchDirection = InputManager.Instance.GetTouchDirectionNomalized();
        if (InputManager.Instance.GetSwipeVelocity() >= InputManager.Instance.minSwipeVelocity)
        {
            OnSliced(touchDirection, touchPosition, 6.0f);
            GameManager.Instance.AddPoint(1);
            UIManager.Instance.UpdatePoint();
        }
       
        
       
    }


    public void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
    {
        
        wholeFruit.SetActive(false);
        slicedFruit.SetActive(true);
        GameManager.Instance.combo++;
        StartCoroutine(GameManager.Instance.ShowCombo());
        float newAngle = Mathf.Atan2(sliceDirection.y, sliceDirection.x) * Mathf.Rad2Deg;
        if (CompareTag("HorizontalSlice"))
        {

            fruitRb.transform.rotation = Quaternion.Euler(0,0
                   , newAngle);
            fruitRb.GetComponent<Collider>().enabled = false;
            slicedFruit.transform.SetParent(null);
            Destroy(slicedFruit, 3.0f);
        }
       else if (CompareTag("VerticalSlice"))
        {
            fruitRb.GetComponent<Collider>().enabled = false;
            // Debug.Log("2");
            slicedFruit.transform.rotation = Quaternion.Euler(0, slicedFruit.transform.localRotation.y + 90, newAngle);
            fruitRb.transform.SetParent(null);
            Destroy(slicedFruit, 3.0f);
        }

        Rigidbody[] sliceRbs = slicedFruit.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in sliceRbs)
        {
           
            rb.velocity = fruitRb.velocity;
            rb.AddForceAtPosition(sliceDirection * force, positionContact, ForceMode.Impulse);
        }
        
    }
    
}
