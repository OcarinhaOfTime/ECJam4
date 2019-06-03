using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    public static SFXManager instance;
    private AudioSource audioSource;

    public AudioClip[] clips;

    private void Awake() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(int id) {
        audioSource.PlayOneShot(clips[id]);
    }
}
