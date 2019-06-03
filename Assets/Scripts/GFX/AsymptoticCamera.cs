using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VUtils;

public class AsymptoticCamera : MonoBehaviour {
    public enum CameraMode {
        Idle,
        Follow,
        FollowGrounded,
        FollowFall
    }

    public CameraMode cameraMode;

    [Range(0, 10)] public float xstiffness = 10f;
    [Range(0, 10)] public float ystiffness = 10f;
    public Transform target;
    public Vector3 offset;
    private Vector3 desiredPos;
    private PlayerController playerController;

    public Rect limit = new Rect(-5, -5, 10, 10);
    private Camera cam;

    private Vector3 targPos {
        get {
            var targ = target.position;
            targ.z = transform.position.z;

            return targ + offset;
        }
    }

    private Vector3 targPosGround {
        get {
            var targ = targPos;
            targ.y = playerController.lastGroundPos.y;
            targ.z = transform.position.z;

            return targ + offset;
        }
    }
    private void Start() {
        playerController = target.GetComponent<PlayerController>();
        cam = GetComponent<Camera>();
    }

    [ContextMenu("Jump to target")]
    public void JumpToTarget() {
        transform.position = targPos;
    }

    private void Follow() {
        var targ = targPos;

        desiredPos.x = transform.position.x + (targ.x - transform.position.x) * xstiffness * Time.deltaTime;
        desiredPos.y = transform.position.y + (targ.y - transform.position.y) * ystiffness * Time.deltaTime;
        desiredPos.z = transform.position.z;
    }

    private void FollowFall() {
        if (playerController.velocity.y >= 0) {
            cameraMode = CameraMode.FollowGrounded;
            return;
        }
        var targ = targPos;

        desiredPos.x = transform.position.x + (targ.x - transform.position.x) * xstiffness * Time.deltaTime;
        desiredPos.y = transform.position.y + (targ.y - transform.position.y) * xstiffness * Time.deltaTime;
        desiredPos.z = transform.position.z;
    }

    private void FollowGrounded() {
        //if(playerController.velocity.y < -25) {
        //    //cameraMode = CameraMode.FollowFall;
        //    return;
        //}
        var targ = targPosGround;

        desiredPos.x = transform.position.x + (targ.x - transform.position.x) * xstiffness * Time.deltaTime;
        desiredPos.y = transform.position.y + (targ.y - transform.position.y) * ystiffness * Time.deltaTime;
        desiredPos.z = transform.position.z;
    }

    private void FixedUpdate() {
        switch (cameraMode) {
            case CameraMode.Idle:
                return;
            case CameraMode.Follow:
                Follow();
                break;
            case CameraMode.FollowGrounded:
                FollowGrounded();
                break;
            case CameraMode.FollowFall:
                FollowFall();
                break;
        }

        desiredPos.y = Mathf.Clamp(desiredPos.y, limit.min.y + cam.orthographicSize, limit.max.y - cam.orthographicSize);
        desiredPos.x = Mathf.Clamp(desiredPos.x, limit.min.x + cam.aspect * cam.orthographicSize, limit.max.x - cam.aspect * cam.orthographicSize);

        transform.position = desiredPos;
    }

    public Coroutine Focus(float duration, Vector3 point) {
        var lastMode = cameraMode;
        cameraMode = CameraMode.Idle;
        point.z = transform.position.z;
        var from = transform.position;

        return this.LerpRoutine(duration, CoTween.SmoothStep, (t) => transform.position = Vector3.Lerp(from, point, t), () => cameraMode = lastMode);
    }
}


