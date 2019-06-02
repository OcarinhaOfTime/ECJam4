using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterseptionRateTest : MonoBehaviour {
    public LineRenderer lr1;
    public LineRenderer lr2;
    public Transform[] intersects;
    public BoxCollider2D rect;

    private IntersectionEvaluator eval;
    public int atk_eval;
    public int def_eval;

    private void Start() {
        eval = GetComponent<IntersectionEvaluator>();
    }

    private void Update() {
        var wmpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lr1.SetPosition(1, wmpos);
        RectCollisionCheck();
        LineCollisionCheck();
    }

    private void RectCollisionCheck() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var r = CollisionUtility.Collider2Rect(rect);
        atk_eval = eval.EvaluateAttack(l1, r);
    }

    public void LineCollisionCheck() {
        var l1 = new CollisionUtility.Line(lr1.GetPosition(0), lr1.GetPosition(1));
        var l2 = new CollisionUtility.Line(lr2.GetPosition(0), lr2.GetPosition(1));
        def_eval = eval.EvaluateDefence(l1, l2);
    }
}
