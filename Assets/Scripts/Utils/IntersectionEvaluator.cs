using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionEvaluator : MonoBehaviour {
    public string[] atk_modifierLabels = { "Regular", "Good", "Great", "Outstanding", "Critical" };
    public float[] atk_modifiers = { 1, 1.1f, 1.2f, 1.3f, 2f };

    public string[] def_modifierLabels = { "Missed", "Good", "Great", "Outstanding", "Perfect" };
    public float[] def_modifiers = { 1, .8f, .6f, .35f, 0f };

    public int EvaluateAttack(CollisionUtility.Line l, CollisionUtility.Rectangle r) {
        var segs = CollisionUtility.LineIntersectsRect(l, r);
        if (segs.Count < 2)
            return 0;

        var d = Vector2.Distance(segs[0], segs[1]);
        var s = r.size * .5f;

        if (d >= s * 1.5f) {
            return atk_modifiers.Length - 1;
        }

        var eval = Mathf.Clamp01(d / s) - .01f;
        return Mathf.FloorToInt(Mathf.Lerp(0, atk_modifiers.Length - 1, eval));
    }

    public int EvaluateDefence(CollisionUtility.Line l1, CollisionUtility.Line l2) {
        Vector2 inter = Vector2.zero;
        var b = CollisionUtility.LineLine(l1, l2, out inter);

        if (!b) {
            return 0;
        }

        var lineLenght = Vector2.Distance(l2.start, l2.end) * .5f;
        var inter2start = Vector2.Distance(l2.start, inter);
        var inter2end = Vector2.Distance(l2.end, inter);

        var lineEvaluationMid = Mathf.Min(inter2start, inter2end) / lineLenght;

        var lineEvaluationAngle = 1 - Mathf.Abs(Vector2.Dot(l1.direction, l2.direction));

        var inter2start0 = Vector2.Distance(l1.start, inter);
        var inter2end0 = Vector2.Distance(l1.end, inter);

        var lineEvaluationSize = Mathf.Clamp01(Mathf.Min(inter2start0, inter2end0) / lineLenght);

        var eval = (lineEvaluationMid * .34f + lineEvaluationAngle * .33f + lineEvaluationSize * .33f);

        //if(eval >= .9) {
        //    return def_modifiers.Length - 1;
        //}

        return Mathf.FloorToInt(Mathf.Lerp(0, def_modifiers.Length, eval));
    }
}
