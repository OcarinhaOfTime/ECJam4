using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomLine : MonoBehaviour {
    [Range(0, .5f)] [SerializeField]private float _width;
    [Range(0, 10)] public float emission = 0;
    [SerializeField]private Color _color = Color.white;
    public float width {
        get {
            return _width;
        }
        set {
            _width = value;
            UpdateMaterial();
        }
    }

    public Color color {
        get {
            return _color;
        }
        set {
            _color = value;
            UpdateMaterial();
        }
    }

    private LineRenderer lr;

    private void Start() {
        lr = GetComponent<LineRenderer>(); ;
    }

    private void OnValidate() {
        UpdateMaterial();
    }

    private void Reset() {
        UpdateMaterial();
    }

    private void UpdateMaterial() {
        var mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_Emission", emission);
        lr = GetComponent<LineRenderer>();
        lr.SetPropertyBlock(mpb);
        lr.startColor = lr.endColor = _color;
        lr.startWidth = lr.endWidth = _width;
    }

    public void SetPositions(Vector2 start, Vector2 end) {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
