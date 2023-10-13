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

    [SerializeField] private Rigidbody topSlicedRb;
    [SerializeField] private Rigidbody botSliceRb;
    [SerializeField] private Blade blade;
    public void Awake()
    {
        collider = GetComponent<Collider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("Blade"))
        // {
        //     other.gameObject.TryGetComponent(out Blade blade);
        //     OnSliced(blade.sliceDirection, blade.transform.position, blade.sliceForce);
        //     GameManager.Instance.AddPoint(1);
        //     UIManager.Instance.UpdatePoint();
        // }
        
    }

    public void OnMouseEnter()
    {
        Vector3 touchPosition = Input.mousePosition;
        Vector3 touchDirection = InputManager.Instance.GetMouseDirectionNomalized();
        Debug.Log("lol");
            // OnSliced(Blade.Instance.sliceDirection, Blade.Instance.transform.position, Blade.Instance.sliceForce);
            OnSliced(touchDirection, touchPosition, Blade.Instance.sliceForce);
            // OnSliced(blade., touch.position, 5.0f);
            GameManager.Instance.AddPoint(1);
            UIManager.Instance.UpdatePoint();
        
       
    }


    public void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
    {
        
        wholeFruit.SetActive(false);
        slicedFruit.SetActive(true);
        float newAngle = Mathf.Atan2(sliceDirection.y, sliceDirection.x) * Mathf.Rad2Deg;
        slicedFruit.transform.rotation = Quaternion.Euler(0, 0, newAngle);

        Rigidbody[] sliceRbs = slicedFruit.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in sliceRbs)
        {
            rb.velocity = fruitRb.velocity;
            rb.AddForceAtPosition(sliceDirection * force, positionContact, ForceMode.Impulse);
        }
        
    }
    
}
