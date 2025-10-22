using System.Collections;
using UnityEngine;

public class PlayerObject : MonoBehaviour {
    
    // Public flags for external access
    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public bool isAttacking = false;
    public bool isDead = false;

    public bool isFalling => velocity.y * up < 0;
    public bool isIdle => velocity.magnitude == 0;
    public bool isJumping => velocity.y * up > 0;
    public bool isWalking => velocity.x != 0;
    public bool walkingRight => velocity.x > 0;
    public float xVelocity => velocity.x;
    public Vector2 velocity => rb.linearVelocity;

    [SerializeField, Tooltip("True for Lower character and False for top Upper character")]
    private bool inverseGravity;
    private Rigidbody2D rb;
    private Vector3 scale;
    private float up;
    private float jumpForce;
    private bool jumpAvailable = false;

    // Character Properties
    [HideInInspector]
    public float maxHorizontalSpeed;
    [HideInInspector]
    public float maxVerticalSpeed;
    [HideInInspector]
    public float fallGravityMultiplier;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        up = inverseGravity ? -1 : 1;
        rb.gravityScale = up;
        scale = transform.localScale;

        PlayerController.Move += move;
        PlayerController.Stop += stop;
        PlayerController.Jump += queueJump;
        PlayerController.Attack += Attack;
        PlayerController.Sprint += SetSprint;
     
    }

    private void Update() {
        if (velocity.y != 0) {
            StopAllCoroutines();
        }

        if (isFalling) {
            if (rb.gravityScale != fallGravityMultiplier) rb.gravityScale = fallGravityMultiplier * up;
            if (Mathf.Abs(velocity.y) > maxVerticalSpeed) rb.linearVelocityY = maxVerticalSpeed * Mathf.Sign(velocity.y);
        }
    }

    private void FixedUpdate() {
        if (jumpAvailable && Grounded()) jump();
    }

    private void move(float force, int direction = 1) {
        if (Mathf.Abs(velocity.x) >= maxHorizontalSpeed) return;
        rb.AddForceX(force * direction);
        if (Mathf.Sign(velocity.x) != direction) stop(force);
    }

    private void stop(float force) {
        if (velocity.x == 0) return;
        if (Mathf.Abs(velocity.x) < .1) rb.linearVelocityX = 0;
        else rb.AddForceX(-force * Mathf.Sign(velocity.x));
    }

    public void die() {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died.");
    }

    private void jump() {
        jumpAvailable = false;
        rb.gravityScale = up;
        rb.linearVelocityY = 0;
        rb.AddForceY(up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("Jump");
    }

    private void queueJump(float force) {
        jumpAvailable = true;
        jumpForce = force;
        jumpBuffer();
    }

    private bool Grounded() {
        Vector2 box = new Vector2 (.39f, .2f);
        return Physics2D.BoxCast(transform.position, box, 0, up*Vector2.down, .5f, LayerMask.GetMask("Platform"));
        //return Physics2D.Raycast(transform.position, Vector3.down*up, .6f, LayerMask.GetMask("Platform"));
    }

    private void Attack() {
        isAttacking = true;
        Debug.Log($"{gameObject.name} attacks!");
        StartCoroutine(ResetAttackFlag());
    }

    private IEnumerator ResetAttackFlag() {
        yield return null;
        isAttacking = false;
    }

    private async void jumpBuffer() {
        await Awaitable.WaitForSecondsAsync(.5f);
        jumpAvailable = false;
    }

    private void SetSprint(bool sprinting) {
        isSprinting = sprinting;
    }

    private void OnDisable() {
        PlayerController.Move -= move;
        PlayerController.Stop -= stop;
        PlayerController.Jump -= queueJump;
        PlayerController.Attack -= Attack;
        PlayerController.Sprint -= SetSprint;
    }
}
