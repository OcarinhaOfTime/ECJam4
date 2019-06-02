using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VUtils;

public class SpheresManager : MonoBehaviour {
    [System.Serializable]
    public class LineConnectEvent : UnityEvent<CollisionUtility.Line> { }
    public MagicSphere spherePrefab;
    public float radius = 2.0f;
    public CustomLine customLine;
    public PointerHandler pointerHandler;

    public UnityEvent<CollisionUtility.Line> onLineConnect = new LineConnectEvent();
    public UnityEvent onConnectFail = new UnityEvent();

    private Camera cam;
    private List<int> selectedPoints = new List<int>();
    private List<MagicSphere> spheres = new List<MagicSphere>();

    private Vector2 wmpos {
        get {
            return cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private float rand {
        get {
            return Random.value * 2 - 1;
        }
    }

    private void Awake() {
        cam = Camera.main;
        pointerHandler.onPointerDown.AddListener(OnPointerDown);
    }

    public void Create(Color[] sphereColors) {
        Clear();

        for (int i = 0; i < sphereColors.Length; i++) {
            var sphere = Instantiate(spherePrefab);
            sphere.Setup();
            sphere.color = sphereColors[i];
            sphere.transform.SetParent(transform);

            sphere.transform.localPosition = new Vector2(rand * radius, rand * radius);
            sphere.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
            sphere.name = "sphere" + i;

            spheres.Add(sphere);
        }
    }

    public void Clear() {
        for (int i = 0; i < spheres.Count; i++) {
            Destroy(spheres[i].gameObject);
        }

        spheres.Clear();
    }

    public void SetAllActive(bool b) {
        for (int i = 0; i < spheres.Count; i++) {
            spheres[i].moving = true;
            spheres[i].gameObject.SetActive(b);
            spheres[i].transform.localPosition = new Vector2(rand * radius, rand * radius);
        }

        selectedPoints.Clear();
    }

    public void OnPointerDown(PointerEventData eventData) {
        var p = wmpos;
        for (int i = 0; i < spheres.Count; i++) {
            if (spheres[i].col.OverlapPoint(p)) {
                SelectSphere(i);
                break;
            }
        }
    }

    private void SelectSphere(int i) {
        if (selectedPoints.Contains(i)) {
            spheres[i].moving = true;
            selectedPoints.Remove(i);
        } else if (selectedPoints.Count == 0) {
            selectedPoints.Add(i);
            spheres[i].moving = false;
        } else {
            selectedPoints.Add(i);
            spheres[i].moving = false;
            if (spheres[i].color == spheres[selectedPoints[0]].color) {
                Connect();
            } else {
                Fail();
            }
        }
    }

    private void Connect() {
        var i1 = selectedPoints[0];
        var i2 = selectedPoints[1];
        customLine.SetPositions(spheres[i1].transform.position, spheres[i2].transform.position);
        customLine.color = spheres[i1].color;
        customLine.width = .25f;

        var line = new CollisionUtility.Line(spheres[i1].transform.position, spheres[i2].transform.position);

        this.ExecAfterSecs(1, () => {
            spheres[i1].gameObject.SetActive(false);
            spheres[i2].gameObject.SetActive(false);
            customLine.width = 0f;
            selectedPoints.Clear();

            onLineConnect.Invoke(line);
        });
    }

    private void Fail() {
        this.ExecAfterSecs(1, () => {
            for (int i = 0; i < selectedPoints.Count; i++) {
                var i1 = selectedPoints[i];
                spheres[i1].gameObject.SetActive(false);
            }
            selectedPoints.Clear();
            onConnectFail.Invoke();
        });
    }
}
