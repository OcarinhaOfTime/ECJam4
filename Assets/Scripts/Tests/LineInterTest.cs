using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInterTest : MonoBehaviour {
    public LineRenderer lr1;
    public LineRenderer lr2;
    public Transform[] intersects;
    public BoxCollider2D rect;

    private void Update() {
        var wmpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lr1.SetPosition(1, wmpos);
        RectCollisionCheck();
    }

    private void LineCollisionCheck() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var l2 = new CollisionUtility.Line(lr2.GetPosition(0), lr2.GetPosition(1));
        Vector2 inter = Vector2.zero;
        var b = CollisionUtility.LineLine(l1, l2, out inter);
        //intersect.position = inter;

        var col = b ? Color.red : Color.white;
        lr1.startColor = lr1.endColor = col;
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
    }

    private void RectCollisionCheck2() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var r = CollisionUtility.Collider2Rect(rect);
        var b = CollisionUtility.LineIntersectsRectTest(l1, r);

        var col = b ? Color.red : Color.white;
        lr1.startColor = lr1.endColor = col;
    }
}
