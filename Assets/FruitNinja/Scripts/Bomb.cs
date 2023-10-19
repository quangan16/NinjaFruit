using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Collider bombCollider;
    [SerializeField] private AudioClip bombExplode;
    [SerializeField] private AudioSource bombFuseSource;

    public void Start()
    {
        bombCollider = GetComponent<Collider>();
        bombFuseSource.Play();
    }
    public void OnMouseOver()
    {
      OnSliced();
    }

    public void OnSliced()
    {
        Explode();
        if (GameManager.Instance.score < 5)
        {
            GameManager.Instance.ResetPoint();
        }
        else
        {
            GameManager.Instance.MinusPoint(5);
        }
        
    }

    public void Explode()
    {
        GameManager.Instance.DestroyAllActiveFruits();
        
    }
}
