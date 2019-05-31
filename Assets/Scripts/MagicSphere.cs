using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSphere : MonoBehaviour {
    public Color color {
        get {
            return sr.color;
        }
        set {
            sr.color = value;
        }
    }
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        
    }
}
