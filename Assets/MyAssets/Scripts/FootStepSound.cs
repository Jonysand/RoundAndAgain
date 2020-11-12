#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomClip(){
        return clips[Random.Range(0, clips.Length)];
    }

    private void PlayStep(){
        if(audioSource.enabled){
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
        }
    }
}
