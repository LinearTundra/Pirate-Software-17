using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerObject po;
    private Animator animator;

    // Cache previous states to avoid redundant animator calls
    private bool prevIsDead, prevIsJumping, prevIsAttacking, prevIsSprinting;
    private float prevXVelocity;

    void Start()
    {
        animator = po.GetComponent<Animator>();
        if (animator == null) Debug.LogWarning("Animator component missing on PlayerObject.");
    }

    void Update()
    {
        if (po.isDead != prevIsDead)
        {
            animator.SetTrigger("isDead");
            prevIsDead = po.isDead;
        }

        if (po.isWalking)
        {
            float absVelocity = Mathf.Abs(po.xVelocity);
            if (Mathf.Abs(absVelocity - prevXVelocity) > 0.01f) 
            {
                animator.SetFloat("Xvelocity", absVelocity);
                prevXVelocity = absVelocity;
            }
        }
        else if (prevXVelocity != 0)
        {
            animator.SetFloat("Xvelocity", 0);
            prevXVelocity = 0;
        }

        if (po.isJumping != prevIsJumping)
        {
            animator.SetBool("isJumping", po.isJumping);
            prevIsJumping = po.isJumping;
        }

        if (po.isAttacking != prevIsAttacking)
        {
            animator.SetBool("isAttacking", po.isAttacking);
            prevIsAttacking = po.isAttacking;
        }

        if (po.isSprinting != prevIsSprinting)
        {
            animator.SetBool("isSprinting", po.isSprinting);
            prevIsSprinting = po.isSprinting;
        }
    }
}
