using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TiledSprite))]
public class ParallaxSprite : MonoBehaviour {
    public float speed = 1;
    public bool fixedy = false;
    private Transform cam;
    private TiledSprite ts;

    private float starty = 0;

    private void Start() {
        cam = Camera.main.transform;
        ts = GetComponent<TiledSprite>();
        starty = transform.position.y;
    }

    private void Update() {
        ts.offset = Vector2.right * speed * cam.position.x * .001f;
        if (fixedy)
            transform.position = new Vector3(transform.position.x, starty, transform.position.z);
    }
}
