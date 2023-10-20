using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G80_SoundManager : MonoBehaviour
{
   public static G80_SoundManager Instance;

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
