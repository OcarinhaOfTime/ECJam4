using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInterTest : MonoBehaviour {
    public LineRenderer lr1;
    public LineRenderer lr2;
    public Transform intersect;

    private void Update() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var l2 = new CollisionUtility.Line(lr2.GetPosition(0), lr2.GetPosition(1));
        Vector2 inter = Vector2.zero;
        var b = CollisionUtility.LineLine(l1, l2, out inter);
        intersect.position = inter;

        var col = b ? Color.red : Color.white;
        lr1.startColor = lr1.endColor = col;
    }
}
