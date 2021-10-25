using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : MonoBehaviour {

    [Header ("Settings")]
    public bool canGlide;
    public bool canDash;
    public bool mustFaceDash;
    public bool canMove = true;

    [Header ("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;
    float lastXInput;
    public bool damageAhead;

    [Header ("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    public bool jumpUp = false;

    [Header ("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public LayerMask damageLayer;
    public GameObject characterHolder;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem Dust;
    public ParticleSystem Landing;
    public ParticleSystem Spark;

    [Header ("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    public bool isGliding = false;
    public bool isDashing = false;
    public float desiredFinalMagnitude = 20;
    bool isTakingDamage;
    public Vector2 RayOffset = Vector2.one;

    [Header ("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;
    public Vector3 lastValidPosition;

    [Header ("External References")]
    public Transform dashTarget;

    RaycastHit2D hit2D;

    // Update is called once per frame
    // Update is called once per frame
    void Update () {
        bool wasOnGround = onGround;

        onGround = Physics2D.Raycast (transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast (transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        StaticEvents.isPlayerGrounded = onGround;

        if (canMove) {

            if (!wasOnGround && onGround) {
                if (isDashing)
                    isDashing = false;
                StartCoroutine (JumpSqueeze (1.5f, 0.5f, 0.15f));
                Land ();
            }

            damageAhead = LookForDamageRaycast ();
            if (onGround && !isTakingDamage && !damageAhead) {
                lastValidPosition = transform.position;
            }

            if (Input.GetButtonDown ("Jump")) {
                jumpTimer = Time.time + jumpDelay;
            }

            //If you're in the air AND you can glide...
            if (!onGround && canGlide) {
                //Check if the Glide Button is pressed.
                if (Input.GetButton ("Jump") && rb.velocity.y < 0) {
                    isGliding = true;
                } else {
                    isGliding = false;
                }
            }

            //If you're in the air AND you can dash AND there is a target to dash towards...
            if (!onGround && canDash && dashTarget != null && !isDashing) {
                //Check if the Glide Button is pressed.
                if (Input.GetButtonDown ("Jump")) {
                    if (isFacingObject (dashTarget) || !mustFaceDash)
                        DashToTarget ();
                }
            }

            animator.SetBool ("onGround", onGround);
            direction = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

        } else {
            direction = Vector2.zero;
        }
    }
    void FixedUpdate () {
        if (canMove && !isDashing) {
            moveCharacter (direction.x);
            if (jumpTimer > Time.time && onGround) {
                Jump ();
            }
        } else {
            animator.SetFloat ("horizontal", 0);
            animator.SetFloat ("vertical", 0);
        }
        modifyPhysics ();
    }
    void moveCharacter (float horizontal) {
        rb.AddForce (Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            Flip ();
        }
        if (Mathf.Abs (rb.velocity.x) > maxSpeed) {
            rb.velocity = new Vector2 (Mathf.Sign (rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat ("horizontal", Mathf.Abs (rb.velocity.x));
        animator.SetFloat ("vertical", rb.velocity.y);
    }
    void Jump () {
        rb.velocity = new Vector2 (rb.velocity.x, 0);
        rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        StartCoroutine (JumpSqueeze (0.5f, 1.2f, 0.1f));
        jumpUp = true;
        CreateDust ();
    }
    void modifyPhysics () {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround) {

            if (Mathf.Abs (direction.x) < 0.4f || changingDirections) {
                rb.drag = linearDrag;
            } else {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;

            hit2D = Physics2D.Raycast (transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
            if (hit2D.collider != null && jumpUp == false) {
                Vector2 pos = this.transform.position;
                pos.y = hit2D.point.y;
                this.transform.position = pos;
            }

            if (canGlide)
                isGliding = false;
        } else {
            if (canGlide && isGliding)
                Glide ();

            rb.gravityScale = gravity;

            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0) {
                rb.gravityScale = gravity * fallMultiplier;
                jumpUp = false;
            } else if ((rb.velocity.y > 0 && !Input.GetButton ("Jump") && jumpUp) || isDashing) {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("SimpleDamage")) {
            //Damage_Teleport();
            isTakingDamage = true;
            Damage_Force (other.contacts[0].point);
        }
    }

    private void OnCollisionExit2D (Collision2D other) {
        if (other.gameObject.CompareTag ("SimpleDamage")) {
            //Damage_Teleport();
            isTakingDamage = false;
        }
    }

    bool LookForDamageRaycast () {

        Vector2 lookDir;
        if (facingRight)
            lookDir = Vector2.right;
        else
            lookDir = Vector2.left;

        Vector3 offset = new Vector3 (0, RayOffset.y, 0);
        hit2D = Physics2D.Raycast (transform.position - offset, lookDir * RayOffset.x, RayOffset.x, damageLayer);
        if (hit2D.collider != null) {
            return true;
        }
        //If there is an object nearby that does damage, don't update the position
        return false;
    }

    void Damage_Force (Vector2 position) {

        float xVel = rb.velocity.x;

        rb.velocity = Vector2.zero;
        Vector3 dest = position;
        Vector2 direction;
        direction.y = 2;

        //if (dest.x > this.transform.position.x) {
        if (lastValidPosition.x < transform.position.x) {
            Debug.Log ("Last: " + lastValidPosition.x + ", Current: " + transform.position.x + ". Moving left!");
            direction.x = -2;
        } else if (lastValidPosition.x > transform.position.x) {
            Debug.Log ("Last: " + lastValidPosition.x + ", Current: " + transform.position.x + ". Moving right!");
            direction.x = 2f;
        } else  {
            Debug.Log ("There's no difference!");
            direction.x = 0f;
        }

        rb.velocity = (direction * 5);
        jumpUp = true;

        CreateDust ();
        StaticEvents.flashSprite.Invoke (spriteRenderer, .25f);
        StartCoroutine (DoDamage_Force ());
    }

    IEnumerator DoDamage_Force () {
        canMove = false;
        yield return new WaitForSeconds (.25f);
        canMove = true;
    }

    void Damage_Teleport () {
        this.transform.position = lastValidPosition;
        StartCoroutine (DamageCooldown ());
    }

    IEnumerator DamageCooldown () {
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        StaticEvents.flashSprite.Invoke (spriteRenderer, .8f);

        yield return new WaitForSeconds (1f);
        rb.isKinematic = false;
        canMove = true;
    }

    void Glide () {
        Vector2 vel = rb.velocity;
        vel.y = -.1f;
        rb.velocity = vel;
    }

    void DashToTarget () {
        StartCoroutine (DoDash ());
    }

    IEnumerator DoDash () {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        DoSpark ();

        yield return new WaitForSeconds (.2f);
        Vector3 dest = dashTarget.position;

        float distance = Vector3.Distance (dest, this.transform.position);
        float speedNeeded = distance / linearDrag + desiredFinalMagnitude;

        Vector3 direction = (dest - this.transform.position).normalized;

        rb.isKinematic = false;
        rb.velocity = (direction * speedNeeded);

        isDashing = true;

        CreateDust ();
    }
    void Flip () {
        facingRight = !facingRight;
        //transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        spriteRenderer.flipX = !facingRight;
    }

    public bool isFacingObject (Transform t) {
        if (transform.position.x < t.position.x) {
            if (facingRight)
                return true;
            else
                return false;
        } else {
            if (!facingRight)
                return true;
            else
                return false;
        }
    }

    public void CheckIfFlipCorrect () {
        spriteRenderer.flipX = !facingRight;
    }

    IEnumerator JumpSqueeze (float xSqueeze, float ySqueeze, float seconds) {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3 (xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp (originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp (newSize, originalSize, t);
            yield return null;
        }

    }
    private void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawLine (transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine (transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);

        Vector2 lookDir;
        if (facingRight)
            lookDir = Vector2.right;
        else
            lookDir = Vector2.left;

        Vector3 offset = new Vector3 (0, RayOffset.y, 0);
        Gizmos.DrawRay (transform.position - offset, lookDir * RayOffset.x);
    }
    void CreateDust () {
        Dust.Play ();
    }
    void Land () {
        Landing.Play ();
    }

    void DoSpark () {
        Spark.Play ();
    }
}