using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VUtils;

public class MusicManager : MonoBehaviour {
    [System.Serializable]
    public class TwoPartSong {
        public AudioClip intro;
        public AudioClip loop;
    }

    public static MusicManager instance;

    public AudioClip[] clips;
    public TwoPartSong[] twoPartSongs;
    private AudioSource audioSource;

    private void SetLoop(bool b) {
        audioSource.loop = b;
    }

    private void Awake() {
        if(instance == null) {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            audioSource = GetComponent<AudioSource>();
        } else {
            Destroy(gameObject);
        }
    }

    public void FadeInOutMusic(int id, float duration = 1, bool loop = true) {
        StartCoroutine(FadeInOutMusicRoutine(id, duration, loop));
    }

    public IEnumerator FadeInOutMusicRoutine(int id, float duration, bool loop = true) {
        yield return this.LerpRoutine(duration * .5f, CoTween.SmoothStep, (t) => audioSource.volume = 1 - t);
        audioSource.Stop();
        audioSource.clip = clips[id];
        audioSource.loop = loop;
        audioSource.Play();
        yield return this.LerpRoutine(duration * .5f, CoTween.SmoothStep, (t) => audioSource.volume = t);
    }

    public void FadeOutMusic(int id, float duration = 0) {
        StartCoroutine(FadeOutMusicRoutine(id, duration));
    }

    public IEnumerator FadeOutMusicRoutine(int id, float duration) {
        yield return this.LerpRoutine(duration, CoTween.SmoothStep, (t) => audioSource.volume = 1 - t);
        audioSource.Stop();
        audioSource.clip = clips[id];
        audioSource.volume = 1;
        audioSource.Play();
    }

    public void FadeInMusic(int id, float duration = .25f) {
        StartCoroutine(FadeInMusicRoutine(id, duration));
    }

    public IEnumerator FadeInMusicRoutine(int id, float duration) {
        audioSource.Stop();
        audioSource.clip = clips[id];
        audioSource.loop = true;
        audioSource.Play();
        yield return this.LerpRoutine(duration, CoTween.SmoothStep, (t) => audioSource.volume = t);
    }

    public void Play2PartSong(int id, float duration = 1) {
        StartCoroutine(Play2PartSongRoutine(id, duration));
    }

    private IEnumerator Play2PartSongRoutine(int id, float duration) {
        var two_part_song = twoPartSongs[id];
        yield return this.LerpRoutine(duration * .5f, CoTween.SmoothStep, (t) => audioSource.volume = 1 - t);
        audioSource.Stop();
        audioSource.clip = two_part_song.intro;
        audioSource.loop = false;
        audioSource.Play();
        yield return this.LerpRoutine(duration * .5f, CoTween.SmoothStep, (t) => audioSource.volume = t);

        while (audioSource.isPlaying)
            yield return null;

        audioSource.clip = two_part_song.loop;
        audioSource.loop = true;
        audioSource.Play();
    }
}
