using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {
    private bool _controlling = true;
    public bool controlling {
        get {
            return _controlling;
        }
        set {
            _controlling = value;
            h_input = 0;
            velocity.x = 0;
        }
    }
    [Foldout("Movement")] public float timeReachMax = .1f;
    [Foldout("Movement")] public float timeReachRunMax = .2f;
    [Foldout("Movement")] public float timeToStop = .1f;
    [Foldout("Movement")] public float max_speed = 10;
    [Foldout("Movement")] public float sprintMaxVel = 5;
    [Foldout("Movement")] public float sprintSmoothness = 10;
    [Foldout("Movement")] [Range(0f, 1)] public float crouchSpeedFactor = .5f;
    [Foldout("Movement")] public float turnSpeed = 10;
    [Foldout("Movement")]
    [Tooltip("How many seconds I need to stop moving to get out of the run animation")]
    public float idleThreshold = .2f;

    [Foldout("Jump")]
    public float jump_height = 5;
    [Foldout("Jump")]
    [Range(0f, 1)]
    public float prematureJumpFactor = .5f;
    [Foldout("Jump")]
    [Tooltip("How much of the first jump height the following jumps will have.")]
    [Range(.5f, 1)]
    public float extra_jump_factor = .5f;
    [Foldout("Jump")]
    public float jump_duration = .33f;
    [Foldout("Jump")]
    public float fallMultiplier = 1;

    [Foldout("Jump")]
    public float time2MaxJump = .5f;
    [Tooltip("Time (in seconds) for the player to still be allowed to jump after start falling.")]
    [Foldout("Jump")] public float jumpTolerance = .2f;
    [Foldout("Jump")] public LayerMask groundMask = (1 << 8);
    [Foldout("Jump")] public int jumps = 2;

    private bool pressingGrabButton;

    public CollisionInfo collisionInfo = new CollisionInfo();


    public Vector2 velocity;
    private Vector2 deltaVelocity;

    public bool facingRight { get; private set; }
    public bool grounded { get; private set; }
    public Vector2 lastGroundPos;

    private float lastDir;
    private float jumpToleranceTimer;
    private bool justLeftGround = false;
    private int jumpCounter;
    private bool start_jump;
    private bool prematureJump;
    private float jumpStartTime;
    private float deltaJumpT;
    private const float skin_width = .01f;
    private RaycastOrigins raycastOrigins;
    private int horizontalRayCount = 4;
    private int verticalRayCount = 4;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private const float MaxClimbSlope = 60;

    private BoxCollider2D col;
    private Vector2 colSize;
    private Vector2 colOffset;
    private Transform pivot;
    private float realMaxSpeed;

    private float maxYSpeed;
    private bool landed;
    private bool walking;

    private float h_input;
    private float v_input;

    private float acceleration {
        get {
            return max_speed / timeReachMax;
        }
    }

    private float desacceleration {
        get {
            return max_speed / timeToStop;
        }
    }

    private float jump_vel {
        get {
            return 2 * jump_height / jump_duration;
        }
    }

    private float extra_jump_vel {
        get {
            return 2 * jump_height / jump_duration;
        }
    }

    public float g {
        get {
            return (-2 * jump_height) / (jump_duration * jump_duration);
        }
    }


    public float facingSign {
        get {
            return facingRight ? 1 : -1;
        }
    }

    public bool facing {
        set {
            pivot.localScale = new Vector3(Mathf.Abs(pivot.localScale.x) * (value ? 1 : -1), pivot.localScale.y, pivot.localScale.z);
        }
    }

    private void Awake() {
        facingRight = true;
        col = GetComponent<BoxCollider2D>();
        pivot = transform;
        realMaxSpeed = max_speed;
    }

    private void Start() {
        colSize = col.size;
        colOffset = col.offset;
        CalculateRaySpacing();
    }

    private void Update() {
        if (grounded)
            lastGroundPos = transform.position;

        if (!controlling)
            return;
        JumpInput();
    }

    private void JumpInput() {
        if (Input.GetButtonDown("Jump")) {
            start_jump = true;
            prematureJump = false;
            jumpStartTime = Time.time;
        }
        if (Input.GetButtonUp("Jump")) {
            deltaJumpT = Time.time - jumpStartTime;
            if (deltaJumpT < time2MaxJump)
                prematureJump = true;
        }
    }

    float Sign(float x) {
        if (x > .01f)
            return 1;

        if (x < -.01f)
            return -1;

        return 0;
    }

    public void Move() {
        if (!controlling)
            return;
        h_input = Input.GetAxisRaw("Horizontal");
        v_input = Input.GetAxisRaw("Vertical");

        if (h_input == 0) {
            velocity.x -= desacceleration * lastDir * Time.fixedDeltaTime;

            if (Sign(velocity.x) != lastDir) {
                lastDir = 0;
                velocity.x = 0;
            }
        } else {
            velocity.x += h_input * acceleration * Time.fixedDeltaTime;
            lastDir = h_input;
        }

        if (start_jump && jumpCounter < jumps) {
            velocity.y = jump_vel * (jumpCounter > 0 ? extra_jump_factor : 1);

            jumpCounter++;
            maxYSpeed = 0;
        }

        start_jump = false;
        if (prematureJump) {
            prematureJump = false;
            velocity.y = jump_vel * prematureJumpFactor + deltaJumpT * g;
        }


        if (h_input != 0) {
            var coff = colOffset;
            coff.x = Mathf.Abs(coff.x) * h_input;
            colOffset = coff;
        }
    }

    private void FixedUpdate() {
        if (collisionInfo.below || collisionInfo.above) {
            velocity.y = 0;
        }

        Move();

        velocity.y += g * Time.deltaTime * (velocity.y < 0 ? fallMultiplier : 1);

        realMaxSpeed = Mathf.SmoothStep(realMaxSpeed, max_speed, Time.deltaTime * sprintSmoothness);
        velocity.x = Mathf.Clamp(velocity.x, -realMaxSpeed, realMaxSpeed);
        deltaVelocity = Time.deltaTime * velocity;

        CollisionHandling();

        if (!grounded && jumpToleranceTimer > jumpTolerance) {
            jumpCounter = Mathf.Max(1, jumpCounter);
        }

        jumpToleranceTimer += Time.deltaTime;
        transform.Translate(deltaVelocity);

        UpdateAnimator();

        AdjustTurn();
    }

    private void CollisionHandling() {
        collisionInfo.Reset();
        UpdateRaycastOrigins();

        if (deltaVelocity.y < 0) {
            DescendSlope();
        }

        if (deltaVelocity.x != 0) {
            HorizontalCollisions();
        }

        if (deltaVelocity.y != 0) {
            VerticalCollisions();
        }
    }    

    private void UpdateAnimator() {
        var absX = Mathf.Abs(velocity.x);
        var maxJog = Mathf.Clamp01(absX / max_speed);
        var sprintVel = Mathf.Clamp01((absX - max_speed) / (sprintMaxVel - max_speed));
        maxYSpeed = Mathf.Min(maxYSpeed, velocity.y);
        maxYSpeed = (maxYSpeed > -15 && maxYSpeed <= -12.5f) ? -12.5f : maxYSpeed;
        maxYSpeed = (absX > 1f ? maxYSpeed : Mathf.Max(maxYSpeed, -12.5f));
        var runSpeed = (sprintVel + maxJog) / 2f;
    }

    private void AdjustTurn() {
        if (facingRight && velocity.x < -.01f || !facingRight && velocity.x > .01f)
            facingRight = !facingRight;

        facing = facingRight;
    }

    void OnLand() {
        jumpCounter = 0;
        var t = Mathf.Abs(maxYSpeed);
        t = Mathf.Max(t - 10, 0);
        t /= 5f;
        if (t > 0) {
            var trauma = Mathf.Lerp(.3f, .75f, t);
            //print("trauma: " + trauma);
        }
    }

    void HorizontalCollisions() {
        var dir = Mathf.Sign(deltaVelocity.x);
        var rayLength = Mathf.Abs(deltaVelocity.x) + skin_width;

        for (int i = 0; i < horizontalRayCount; i++) {
            var pos = dir < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            pos += Vector2.up * horizontalRaySpacing * i;

            var hit = Physics2D.Raycast(pos, Vector2.right * dir, rayLength, groundMask);
            Debug.DrawRay(pos, Vector2.right * dir * rayLength, Color.red);

            if (hit) {
                var angle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && angle <= MaxClimbSlope) {
                    var distanceToSlopeStart = 0f;
                    if (collisionInfo.slopeAngleOld != angle) {
                        distanceToSlopeStart = hit.distance - skin_width;
                        deltaVelocity.x -= distanceToSlopeStart * dir;
                    }
                    ClimbSlope(angle);
                    deltaVelocity.x += distanceToSlopeStart * dir;
                }

                if (!collisionInfo.climbingSlope || angle > MaxClimbSlope) {
                    deltaVelocity.x = (hit.distance - skin_width) * dir;
                    rayLength = hit.distance;

                    if (collisionInfo.climbingSlope) {
                        deltaVelocity.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(deltaVelocity.x);
                    }

                    collisionInfo.left = dir < 0;
                    collisionInfo.right = dir > 0;
                    collisionInfo.horizontalHit = hit.collider;
                }
            }
        }
    }

    void ClimbSlope(float slopeAngle) {
        float moveDistance = Mathf.Abs(deltaVelocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (deltaVelocity.y <= climbVelocityY) {
            deltaVelocity.y = climbVelocityY;
            deltaVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(deltaVelocity.x);
            collisionInfo.below = true;
            collisionInfo.climbingSlope = true;
            collisionInfo.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope() {
        float dir = Mathf.Sign(deltaVelocity.x);
        Vector2 pos = (dir == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(pos, -Vector2.up, Mathf.Infinity, groundMask);

        if (hit) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= MaxClimbSlope) {
                if (hit.normal.x * dir > 0) {
                    var moveDistance = Mathf.Abs(deltaVelocity.x);
                    if (hit.distance - skin_width <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * moveDistance) {
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        deltaVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * dir;
                        deltaVelocity.y -= descendVelocityY;

                        collisionInfo.slopeAngle = slopeAngle;
                        collisionInfo.descendingSlope = true;
                        collisionInfo.below = true;
                    }
                }
            }
        }
    }


    void VerticalCollisions() {
        var dir = Mathf.Sign(deltaVelocity.y);
        var rayLength = Mathf.Abs(deltaVelocity.y) + skin_width;

        grounded = false;

        for (int i = 0; i < verticalRayCount; i++) {
            var pos = dir < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            pos += Vector2.right * (verticalRaySpacing * i + deltaVelocity.x);

            var hit = Physics2D.Raycast(pos, Vector2.up * dir, rayLength, groundMask);
            Debug.DrawRay(pos, Vector2.up * dir * rayLength, Color.red);

            if (hit) {
                deltaVelocity.y = (hit.distance - skin_width) * dir;
                rayLength = hit.distance;

                if (collisionInfo.climbingSlope) {
                    deltaVelocity.x = Mathf.Abs(deltaVelocity.y) / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(deltaVelocity.x);
                }

                collisionInfo.below = dir < 0;
                collisionInfo.above = dir > 0;
            }
        }

        if (collisionInfo.climbingSlope) {
            dir = Mathf.Sign(deltaVelocity.x);
            rayLength = Mathf.Abs(deltaVelocity.x) + skin_width;
            var pos = dir < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight + Vector2.up * deltaVelocity.y;
            var hit = Physics2D.Raycast(pos, Vector2.right * dir, rayLength, groundMask);

            if (hit) {
                var angle = Vector2.Angle(hit.normal, Vector2.up);
                if (angle != collisionInfo.slopeAngle) {
                    deltaVelocity.x = (hit.distance - skin_width) * dir;
                    collisionInfo.slopeAngle = angle;
                }
            }
        }

        if (collisionInfo.below) {
            if (!landed) {
                landed = true;
                OnLand();
            }
            justLeftGround = false;
            grounded = true;
        } else if (!justLeftGround) {
            OnJustLeftGround();
            justLeftGround = true;
            landed = false;
        }
    }

    void OnJustLeftGround() {
        jumpToleranceTimer = 0;
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = col.bounds;
        bounds.Expand(skin_width * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing() {
        Bounds bounds = col.bounds;
        bounds.Expand(skin_width * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    [System.Serializable]
    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle;
        public float slopeAngleOld;
        public Collider2D horizontalHit;

        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
