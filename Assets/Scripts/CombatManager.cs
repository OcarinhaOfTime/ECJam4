using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {
    public Color[] sphereColors = { Color.red, Color.blue, Color.yellow };
    public MagicSphere spherePrefab;
    public Transform target;
    public Transform spheresParent;

    public int spheres = 6;
    public float spheresRadius = 1;

    private List<MagicSphere> magicSpheres = new List<MagicSphere>();
    private float rand {
        get {
            return Random.value * 2 - 1;
        }
    }

    private void Start() {
        for (int i = 0; i < spheres; i++) {
            var sphere = Instantiate(spherePrefab);
            sphere.gameObject.SetActive(true);
            sphere.color = sphereColors[i % sphereColors.Length];
            sphere.transform.SetParent(spheresParent);

            sphere.transform.localPosition = new Vector2(rand * 2.5f, rand * 2.5f);

            magicSpheres.Add(sphere);
        }
    }
}
