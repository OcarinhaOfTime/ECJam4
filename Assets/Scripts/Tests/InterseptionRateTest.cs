using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterseptionRateTest : MonoBehaviour {
    public LineRenderer lr1;
    public LineRenderer lr2;
    public Transform[] intersects;
    public BoxCollider2D rect;

    public float attackEvaluation;
    public float d;
    public float s;
    public int interCount;

    public string[] modifierLabels = { "Regular", "Good", "Great", "Outstanding", "Critical" };
    public float[] modifiers = { 1, 1.1f, 1.2f, 1.3f, 2f };
    public int index;

    private void Update() {
        var wmpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lr1.SetPosition(1, wmpos);
        RectCollisionCheck();
    }

    private void RectCollisionCheck() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var r = CollisionUtility.Collider2Rect(rect);
        var segs = CollisionUtility.LineIntersectsRect(l1, r);

        var col = segs.Count > 0 ? Color.red : Color.white;
        lr1.startColor = lr1.endColor = col;

        for (int i = 0; i < segs.Count && i < 2; i++) {
            intersects[i].position = segs[i];
        }

        interCount = segs.Count;

        d = Vector2.Distance(intersects[0].position, intersects[1].position);
        s = Vector2.Distance(rect.bounds.min, rect.bounds.max) * .5f;

        if(d >= s * 1.5f) {
            index = modifiers.Length - 1;
        } else {
            attackEvaluation = Mathf.Clamp01(d / s) - .01f;
            index = Mathf.FloorToInt(Mathf.Lerp(0, modifiers.Length - 1, attackEvaluation));
        }        
    }

    public float lineEvaluation;

    public void LineCollisionCheck() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var l2 = new CollisionUtility.Line(lr2.GetPosition(0), lr2.GetPosition(1));
        Vector2 inter = Vector2.zero;
        var b = CollisionUtility.LineLine(l1, l2, out inter);
        //intersect.position = inter;

        var col = b ? Color.red : Color.white;
        lr1.startColor = lr1.endColor = col;
    }
}
