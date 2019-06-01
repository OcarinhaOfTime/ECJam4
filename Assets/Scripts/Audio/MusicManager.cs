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

    public int idTest;
    public float durationTest = .5f;

    public void SetLoop(bool b) {
        audioSource.loop = b;
    }

    private void Awake() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("test")]
    private void SwitchMusic() {
        Play2PartSong(idTest, durationTest);
    }

    public void FadeInOutMusic(int id, float duration = 1) {
        StartCoroutine(FadeInOutMusicRoutine(id, duration));
    }

    public IEnumerator FadeInOutMusicRoutine(int id, float duration) {
        yield return this.LerpRoutine(duration * .5f, CoTween.SmoothStep, (t) => audioSource.volume = 1 - t);
        audioSource.Stop();
        audioSource.clip = clips[id];
        audioSource.loop = true;
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
