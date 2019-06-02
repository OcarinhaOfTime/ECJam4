using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReflectableCamera : MonoBehaviour {
    public const string reflectionTexName = "_GlobalReflectionTex";

    private void OnEnable() {
        var cam = GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0);
        rt.wrapMode = TextureWrapMode.Repeat;
        if (cam.targetTexture != null) {
            var temp = cam.targetTexture;
            cam.targetTexture = null;
            DestroyImmediate(temp);
        }

        cam.targetTexture = rt;
        Shader.SetGlobalTexture(reflectionTexName, cam.targetTexture);
    }
}
