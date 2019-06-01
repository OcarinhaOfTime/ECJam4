using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour {
    [System.Serializable]
    public struct SFXGroup {
        public string groupName;
        public AudioClip[] clips;

        public AudioClip clip {
            get {
                return clips[Random.Range(0, clips.Length)];
            }
        }
    }

    public SFXGroup[] groups;

    private Dictionary<string, SFXGroup> groupsDict = new Dictionary<string, SFXGroup>();

    private AudioSource audioSource;
    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        foreach (var g in groups)
            groupsDict.Add(g.groupName, g);
    }

    public void Play(string sfxName) {
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(groupsDict[sfxName].clip);        
    }
}
