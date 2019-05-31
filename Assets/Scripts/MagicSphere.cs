﻿using System.Collections;
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
    public float randomness = 30;
    public float distThres = 1;
    public Vector2 dir;
    public Vector2 vel;
    public float speed = 1;
    private float timer;
    public float refreshRate = .25f;
    public bool moving = true;
    public CircleCollider2D col { get; private set; }

    public Vector2 position {
        get {
            return transform.localPosition;
        }
        set {
            transform.localPosition = value;
        }
    }

    private void Awake() {
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        speed += Random.value * .5f;
    }

    private void Start() {
        dir = Quaternion.Euler(0, 0, Random.value * 360) * Vector2.right;
        vel = dir * speed;
    }

    private void Update() {
        if (!moving) {
            return;
        }

        if(position.magnitude > distThres && timer > refreshRate) {
            ChangeDir();
        }

        timer += Time.deltaTime;
        position += vel * Time.deltaTime;
    }

    private void ChangeDir() {
        timer = 0;
        if(Random.value < .33f) {
            dir = Quaternion.Euler(0, 0, Random.value * randomness) * - position.normalized;
        } else {
            dir = Quaternion.Euler(0, 0, Random.value * randomness) * -dir;
        }
        vel = dir * speed;
    }
}
