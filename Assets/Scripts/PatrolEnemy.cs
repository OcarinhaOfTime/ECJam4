using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : Character {
    public float speed = 2;
    public float range = 3;

    private Vector3 origin;
    private Vector3 right {
        get {
            return origin + Vector3.right * range;
        }
    }
    private Vector3 left {
        get {
            return origin - Vector3.right * range;
        }
    }

    protected bool facingRight {
        set {
            transform.localScale = value ? Vector3.one : new Vector3(-1, 1, 1);
        }
        get {
            return transform.localScale.x > 0;
        }
    }

    protected float facingSign {
        get {
            return facingRight ? 1 : -1;
        }
    }

    protected Vector2 facingDir {
        get {
            return facingRight ? Vector2.right : -Vector2.right;
        }
    }

    private void Start() {
        origin = transform.position;
        GetComponent<EventTrigger>().onTriggerEnter.AddListener(OnTriggerEnter);
        var sr = GetComponent<SpriteRenderer>();
        sr.color = data.color;
        sr.sprite = data.sprite;

        sr.flipX = true;
    }

    private bool moving = true;

    private void OnTriggerEnter() {
        print("battle time");
        GameManager.instance.StartCombat(this);
        moving = false;
    }

    private void Update() {
        if (!moving || GameManager.instance.inMenu)
            return;
        if (((transform.position.x <= left.x) && !(facingRight)) || ((transform.position.x >= right.x) && (facingRight))) {
                facingRight = !(facingRight);
        }

        transform.position += Vector3.right * facingSign * speed * Time.deltaTime;
    }
}
