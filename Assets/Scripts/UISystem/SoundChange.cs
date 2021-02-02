using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour
{
    public AudioClip bgm;

    private void Start()
    {
        DontDestroyThisObject.Instance.audioSource.clip = bgm;
        DontDestroyThisObject.Instance.audioSource.Play();
    }
}
