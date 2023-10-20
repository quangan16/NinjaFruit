using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class G80_SplashJuice : MonoBehaviour
{
   [SerializeField] private SpriteRenderer _spriteRenderer;

   public void OnDestroy()
   {
       transform.DOKill();
   }

   public void OnEnable()
   {
       FadeOut(1.0f);
   }

   public void FadeOut(float time)
   {
       _spriteRenderer.DOFade(0, time).SetDelay(2.0f).OnComplete(()=>Destroy(gameObject));
   }
}
