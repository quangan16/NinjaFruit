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
    [SerializeField] private ParticleSystem juiceParticle;
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
        fruitRb.GetComponent<Collider>().enabled = false;
        wholeFruit.SetActive(false);
        slicedFruit.SetActive(true);
        juiceParticle.Play();
        GameManager.Instance.combo++;
        if (GameManager.Instance.combo >= 3)
        {
            GameManager.Instance.lastFruitHitPos = transform.position;
        }
        StopAllCoroutines();
        StartCoroutine(GameManager.Instance.ShowCombo());
        float newAngle = Mathf.Atan2(sliceDirection.y, sliceDirection.x) * Mathf.Rad2Deg;
        if (CompareTag("HorizontalSlice"))
        {
            if ((transform.rotation.eulerAngles.z <= 90 && transform.rotation.eulerAngles.z >= 0) ||
                (transform.rotation.eulerAngles.z <= 360 &&
                 transform.rotation.eulerAngles.z >= 270))
            {
                fruitRb.transform.rotation = Quaternion.Euler(0, 0
                    , (newAngle + 180)%180);
            }
            else
            {
                fruitRb.transform.rotation = Quaternion.Euler(0, 0
                    , (newAngle - 180) % 180);
            }
           
         
            slicedFruit.transform.SetParent(null);
            Destroy(slicedFruit, 3.0f);
        }
       else if (CompareTag("VerticalSlice"))
        {
            // Debug.Log("2");
            Debug.Log(transform.rotation.eulerAngles);
            slicedFruit.transform.rotation = Quaternion.Euler(0, slicedFruit.transform.localRotation.y + 90, newAngle);
            slicedFruit.transform.SetParent(null);
            Destroy(slicedFruit, 3.0f);
        }

        Rigidbody[] sliceRbs = slicedFruit.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in sliceRbs)
        {
           
        
            rb.AddForceAtPosition(sliceDirection * force, positionContact, ForceMode.Impulse);
            if (rb.gameObject.CompareTag("TopSliced"))
            {
                rb.AddRelativeForce(transform.up * 50, ForceMode.VelocityChange);
                rb.AddRelativeTorque(new Vector3(-600, 0.0f, 0), ForceMode.VelocityChange);
            }

            else if((rb.gameObject.CompareTag("BotSliced")))
            {
                rb.AddRelativeForce(transform.up * -50, ForceMode.VelocityChange);
                rb.AddRelativeTorque(new Vector3(600f, 0.0f, 0), ForceMode.VelocityChange);
            }
        }
        
    }
    
}
