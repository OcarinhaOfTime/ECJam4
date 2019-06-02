using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EmissiveSprite : MonoBehaviour {
    [Range(0, 10)] public float emission = 0;

    private void OnValidate() {
        UpdateMaterial();
    }

    private void Reset() {
        UpdateMaterial();
    }

    private void UpdateMaterial() {
        var sr = GetComponent<SpriteRenderer>();
        var mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_Emission", emission);
        sr.SetPropertyBlock(mpb);
    }
}
