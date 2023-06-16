using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   [SerializeField] private AudioSource myAudioSource;
   [SerializeField] private ScriptableObject_SoundData soundData;

   public void PlayBallMovingPlay()
   {
      myAudioSource.volume = 0.5f;
      myAudioSource.PlayOneShot(soundData.BallMovingSound);
   }

   public void PlayClickMovingPartSound()
   {
       myAudioSource.volume = 1;
       myAudioSource.PlayOneShot(soundData.ClickMovingPartSound);
   }

   public void PlayWinSound()
   {
       myAudioSource.volume = 1;
       myAudioSource.PlayOneShot(soundData.WinSound);
   }
}
