using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour {
    public float speed = 1;

    private RawImage image;

    private void Start() {
        image = GetComponent<RawImage>();
    }

    private void Update() {
        var rect = image.uvRect;
        rect.position += Vector2.right * Time.deltaTime * speed;
        image.uvRect = rect;
    }
}
