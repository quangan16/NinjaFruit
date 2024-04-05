using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class G80_SoundManager : MonoBehaviour
{
   public static G80_SoundManager Instance;
   public Sfx sfx;

   public void Awake()
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
   public AudioSource audioSrc;
   public void PlaySoundOnce(AudioClip clip)
   {
      audioSrc.PlayOneShot(clip);
      
   }

   public void PlaySoundDelay(AudioClip clip, float delay, bool ignoreTimescale = false)
   {
      DOVirtual.DelayedCall( delay, () => PlaySoundOnce(clip), ignoreTimescale);
   }
   

   public void StopAllSound()
   {
      audioSrc.Stop();
   }
}
[Serializable]
public class Sfx
{
   public AudioClip readyClip;
   public AudioClip startClip;
   public AudioClip launchClip;
}
