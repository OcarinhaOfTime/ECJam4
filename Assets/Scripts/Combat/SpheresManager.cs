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
        customLine.width = 0;
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
        var min_dist = 666f;
        var min_index = -1;
        for (int i = 0; i < spheres.Count; i++) {
            if (spheres[i].col.OverlapPoint(p)) {
                var d = Vector2.Distance(p, spheres[i].position);
                if(d < min_dist) {
                    min_dist = d;
                    min_index = i;
                }
            }
        }

        if(min_index >= 0)
            SelectSphere(min_index);
    }

    private void SelectSphere(int i) {
        SFXManager.instance.PlayClip(2);
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

        this.LerpRoutine(.15f, CoTween.SmoothStep, (t) => customLine.width = t * .1f);

        var line = new CollisionUtility.Line(spheres[i1].transform.position, spheres[i2].transform.position);

        this.ExecAfterSecs(1.5f, () => {
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

    public void SetUnselectedActive() {

    }
}
