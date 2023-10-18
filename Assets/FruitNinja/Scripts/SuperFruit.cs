using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFruit : MonoBehaviour
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
    // Start is called before the first frame update
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
        
    }


}
