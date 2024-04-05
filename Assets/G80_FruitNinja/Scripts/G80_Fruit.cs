using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class G80_Fruit : MonoBehaviour
{
    
    [SerializeField] private Rigidbody fruitRb;
    [SerializeField] private Collider collider;
    [SerializeField] private GameObject wholeFruit;

    [SerializeField] private GameObject slicedFruit;
    [SerializeField] private ParticleSystem juiceParticle;
    [SerializeField] private AudioClip[] slicedSfx;
    [SerializeField] private SpriteRenderer splashJuice;
    public void Awake()
    {
        collider = GetComponent<Collider>();
    }


    public void OnMouseOver()
    {
        Vector3 touchPosition = G80_InputManager.Instance.GetTouchPosition();
        Vector3 touchDirection = G80_InputManager.Instance.GetTouchDirectionNomalized();
        if (G80_InputManager.Instance.GetSwipeVelocity() >= G80_InputManager.Instance.minSwipeVelocity)
        {
            OnSliced(touchDirection, touchPosition, 6.0f);
          
        }
       
    }


    public virtual void OnSliced(Vector3 sliceDirection, Vector3 positionContact, float force)
    {
        fruitRb.GetComponent<Collider>().enabled = false;
        wholeFruit.SetActive(false);
        slicedFruit.SetActive(true);
        juiceParticle.Play();
        G80_GameManager.Instance.combo++;
        G80_GameManager.Instance.AddPoint(1);
        G80_UIManager.Instance.UpdatePoint();
        int randomSoundSfx = Random.Range(0, 3);
        G80_SoundManager.Instance.PlaySoundOnce(slicedSfx[randomSoundSfx]);
        ShowSplashJuice();
        if (G80_GameManager.Instance.combo >= 3)
        {
            G80_GameManager.Instance.lastFruitHitPos = transform.position;
        }
        StopAllCoroutines();
        StartCoroutine(G80_GameManager.Instance.ShowCombo());
        float newAngle = Mathf.Atan2(sliceDirection.y, sliceDirection.x) * Mathf.Rad2Deg;
        if (CompareTag("HorizontalSlice"))
        {
            fruitRb.transform.rotation = Quaternion.Euler(0, 0
                , (newAngle));
         
            slicedFruit.transform.SetParent(null);
            Destroy(slicedFruit, 3.0f);
        }
       else if (CompareTag("VerticalSlice"))
       {
            slicedFruit.transform.localRotation *= Quaternion.Euler(newAngle + 90, 1, 1);
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
                if (CompareTag("VerticalSlice"))
                    rb.AddRelativeTorque(new Vector3(600, 0.0f, 0), ForceMode.VelocityChange);
                else
                    rb.AddRelativeTorque(new Vector3(-600, 0.0f, 0), ForceMode.VelocityChange);
            }

            else if((rb.gameObject.CompareTag("BotSliced")))
            {
                rb.AddRelativeForce(transform.up * -50, ForceMode.VelocityChange);
                rb.AddRelativeTorque(new Vector3(600f, 0.0f, 0), ForceMode.VelocityChange);
            }
        }
        
    }

    public void OnDestroy()
    {
        G80_GameManager.Instance.activeFruits.Remove(gameObject);
    }

    public void ShowSplashJuice()
    {
        splashJuice.gameObject.SetActive(true);
       
        splashJuice.transform.position = transform.position - Vector3.forward * transform.position.z;
        splashJuice.gameObject.transform.SetParent(null);
        splashJuice.gameObject.transform.localScale = Vector3.one * 25.0f;
       
    }
}
