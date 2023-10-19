using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;

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

   public void StopAllSound()
   {
      audioSrc.Stop();
   }
}
