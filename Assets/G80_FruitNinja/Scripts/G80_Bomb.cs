using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G80_Bomb : MonoBehaviour
{
    [SerializeField] private Collider bombCollider;
    [SerializeField] private AudioClip bombExplode;
    [SerializeField] private AudioSource bombFuseSource;
    [SerializeField] private ParticleSystem explodeParticle;
    [SerializeField] private GameObject bombVisual;
    private Rigidbody bombRb;

    public void Start()
    {
        bombRb = GetComponent<Rigidbody>();
        bombCollider = GetComponent<Collider>();
    }
    public void OnMouseOver()
    {
      OnSliced();
    }

    public void OnSliced()
    {
        Explode();
        if (G80_GameManager.Instance.score < 5)
        {
            G80_GameManager.Instance.ResetPoint();
        }
        else
        {
            G80_GameManager.Instance.MinusPoint(5);
        }
        G80_UIManager.Instance.UpdatePoint();
        
        
    }

    public void Explode()
    {
        bombVisual.SetActive(false);
        bombCollider.enabled = false;
        bombRb.isKinematic = true;
        G80_GameManager.Instance.ShakeCamara();
        G80_SoundManager.Instance.PlaySoundOnce(bombExplode);
        explodeParticle.Play();
        PushOtherObjectsAway();
        // GameManager.Instance.DestroyAllActiveFruits();
       
    }

    public void PushOtherObjectsAway()
    {
        foreach (var item in G80_GameManager.Instance.activeFruits)
        {
            if (item.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce((rb.position - transform.position).normalized* 400.0f, ForceMode.VelocityChange);
            }
        }
    }

    public void OnDestroy()
    {
        G80_GameManager.Instance.activeFruits.Remove(gameObject);
    }
}
