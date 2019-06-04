using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VUtils;

public class EnemyAttack : MonoBehaviour {
    public float radius = 1;
    public SpriteRenderer sphere1;
    public SpriteRenderer sphere2;
    public CustomLine line;
    public Color color = Color.red;

    private float rand {
        get {
            return Random.value * 2 - 1;
        }
    }

    public void Clear() {
        sphere1.gameObject.SetActive(false);
        sphere2.gameObject.SetActive(false);
        line.width = .0f;
    }

    public void ActivateRandomAttack(System.Action<CollisionUtility.Line> onEnd) {
        sphere1.color = sphere2.color = line.color = color;
        Vector2 start = Random.insideUnitCircle * radius * Random.Range(.9f, 1.1f);
        Vector2 end = Quaternion.Euler(0, 0, rand * 30) * -start * Random.Range(.9f, 1.1f);

        sphere1.transform.localPosition = start;
        sphere2.transform.localPosition = end;
        line.SetPositions(sphere1.transform.position, sphere2.transform.position);

        StartCoroutine(ActivateRandomAttackRoutine(onEnd));
    }

    private IEnumerator ActivateRandomAttackRoutine(System.Action<CollisionUtility.Line> onEnd) {
        sphere1.gameObject.SetActive(true);
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => sphere1.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t));
        yield return new WaitForSeconds(.5f);
        sphere2.gameObject.SetActive(true);
        yield return this.LerpRoutine(.25f, CoTween.SmoothStep, (t) => sphere2.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t));
        yield return new WaitForSeconds(2f);
        yield return this.LerpRoutine(.15f, CoTween.SmoothStep, (t) => line.width = .1f * t);
        yield return new WaitForSeconds(2f);

        sphere1.gameObject.SetActive(false);
        sphere2.gameObject.SetActive(false);
        line.width = .0f;

        onEnd.Invoke(new CollisionUtility.Line(sphere1.transform.position, sphere2.transform.position));
    }
}
