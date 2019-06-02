using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour {
    public SpriteRenderer sr;
    public BoxCollider2D col;
    public Animator animator;
    [HideInInspector]public CharacterData characterData;

    public float alpha {
        set {
            var c = sr.color;
            c.a = value;
            sr.color = c;
        }
    }

    public void Setup(CharacterData characterData) {
        this.characterData = characterData;
        sr.sprite = characterData.sprite;
        sr.color = characterData.color;
        animator.runtimeAnimatorController = characterData.animator;
    }
}
