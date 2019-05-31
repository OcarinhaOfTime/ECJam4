using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VUtils;

public class CombatManager : MonoBehaviour {
    public Color[] sphereColors = { Color.red, Color.blue, Color.yellow };
    public MagicSphere spherePrefab;
    public Transform target;
    public Transform spheresParent;

    public int spheres = 6;
    public float spheresRadius = 1;

    private List<MagicSphere> magicSpheres = new List<MagicSphere>();
    public PointerHandler pointerHandler;
    public CustomLine customLine;
    private Camera cam;
    public List<int> selectedPoints = new List<int>();

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

    private void Start() {
        cam = Camera.main;

        for (int i = 0; i < spheres; i++) {
            var sphere = Instantiate(spherePrefab);
            sphere.gameObject.SetActive(true);
            sphere.color = sphereColors[i % sphereColors.Length];
            sphere.transform.SetParent(spheresParent);

            sphere.transform.localPosition = new Vector2(rand * 2.5f, rand * 2.5f);
            sphere.name = "sphere" + i;

            magicSpheres.Add(sphere);
        }

        pointerHandler.onPointerDown.AddListener(OnPointerDown);
    }

    public void OnPointerDown(PointerEventData eventData) {
        var p = wmpos;
        for (int i = 0; i < magicSpheres.Count; i++) {
            if (magicSpheres[i].col.OverlapPoint(p)) {
                SelectSphere(i);
                print("overlapssss " + magicSpheres[i].name);
                break;
            }
        }
    }

    private void SelectSphere(int i) {
        if (selectedPoints.Contains(i)) {
            print("Contains!!!");
            magicSpheres[i].moving = true;
            selectedPoints.Remove(i);
        } else if(selectedPoints.Count == 0){
            selectedPoints.Add(i);
            magicSpheres[i].moving = false;
        } else {
            selectedPoints.Add(i);
            magicSpheres[i].moving = false;
            if (magicSpheres[i].color == magicSpheres[selectedPoints[0]].color) {
                print("Attack!!");
                Attack();
            } else {
                print("Miss!!");
                Miss();
            }

        }
    }

    private void Attack() {
        var i1 = selectedPoints[0];
        var i2 = selectedPoints[1];
        customLine.SetPositions(magicSpheres[i1].transform.position, magicSpheres[i2].transform.position);
        customLine.color = magicSpheres[i1].color;
        customLine.width = .25f;

        var boxcol = target.GetComponent<BoxCollider2D>();
        //boxcol.Raycast()

        this.ExecAfterSecs(1, () => {
            magicSpheres[i1].gameObject.SetActive(false);
            magicSpheres[i2].gameObject.SetActive(false);
            customLine.width = 0f;
            selectedPoints.Clear();
        });
    }

    private void Miss() {
        this.ExecAfterSecs(1, () => {
            var i1 = selectedPoints[0];
            var i2 = selectedPoints[1];
            magicSpheres[i1].gameObject.SetActive(false);
            magicSpheres[i2].gameObject.SetActive(false);
            selectedPoints.Clear();
        });
        
    }
}
