using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {
    public UnityEvent onTriggerEnter = new UnityEvent();
    private Collider2D c2d;
    private Collider2D player;
    public bool destroyOnTrigger;
    public bool disableOnTrigger;

    private bool inside;
    private void Start() {
        c2d = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    private void FixedUpdate() { 
        var overlaps = c2d.bounds.Intersects(player.bounds);
        if (overlaps && !inside) {
            print("on trigger enter");
            //on enter
            inside = true;
            onTriggerEnter.Invoke();
            if (destroyOnTrigger)
                Destroy(gameObject);
            if (disableOnTrigger)
                enabled = false;
        }else if (overlaps) {
            //print("on trigger stay");
            //on stay
        } else if(inside) {
            print("on trigger exit");
            //on exit
            inside = false;
        }
    }
}
