using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TiledSprite : MonoBehaviour {
    public Vector2 scale = Vector2.one;
    public Vector2 offset = Vector2.zero;
    private SpriteRenderer sr;

    private int _ScaleOffsetID = Shader.PropertyToID("_ScaleOffset");

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        var mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        mpb.SetVector(_ScaleOffsetID, new Vector4(scale.x, scale.y, offset.x, offset.y));
        sr.SetPropertyBlock(mpb);
    }
}
