using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    
    private enum MovementState { idle, running, jumping, falling, death, meleeAttacking, dash };
    private PlayerMovement playerMovement;
    [HideInInspector] public bool inDialog;
    [HideInInspector] public bool playerLookingLeft;
    public bool IsAttacking { get; private set; } = false;
    public bool IsDiying { get; private set; } = false;
    public bool IsDashing { get; private set; } = false;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (IsDiying)
        {
            // No cambiar de estado mientras el jugador está muriendo
            animator.SetInteger("State", (int)MovementState.death);
            return;
        }
        else if(IsDashing)
        {
            state = MovementState.dash;
            animator.SetInteger("State", (int)MovementState.dash);
            return;
        }
        if (playerMovement.dirX > 0f)
        {
            state = MovementState.running;
            playerLookingLeft = false;
            transform.localScale = new Vector3(1f, 1f, 1f); // mirar derecha

        }
        else if (playerMovement.dirX < 0f)
        {
            state = MovementState.running;
            playerLookingLeft = true;
            transform.localScale = new Vector3(-1f, 1f, 1f); // mirar izquierda
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f && !gameObject.GetComponent<PlayerJumpGlide>().isGrounded())
        {
            animator.StopPlayback();
            state = MovementState.jumping;
        }

        else if (rb.velocity.y < -0.1f)
        {          
            state = MovementState.falling; 

        }
        if(inDialog)
        {
            state = MovementState.idle;
        }
        
        animator.SetInteger("State", (int)state);
        //print($"Current Animation State: {state}");
    }
    public void TriggerMeleeAttackAnimation()
    {
        IsAttacking = true;
        animator.SetInteger("State", (int)MovementState.meleeAttacking);
        StartCoroutine(ResetAfterAttack());
    }
    public void TriggerDeathAnimation()
    {
        print("Triggering Death Animation");
        IsDiying = true;
        animator.SetInteger("State", (int)MovementState.death);
        StartCoroutine(ResetAfterDeath());
    }
    public void TriggerDashAnimation()
    {
        //print("Triggering Dash Animation");
        IsDashing = true;
        animator.SetInteger("State", (int)MovementState.dash);
        StartCoroutine(ResetAfterDash());
    }

    private IEnumerator ResetAfterAttack()
    {
        yield return new WaitForSeconds(0.5f); // Duración real del ataque
        IsAttacking = false;
        UpdateAnimationState();
    }
    private IEnumerator ResetAfterDeath()
    {
        yield return new WaitForSeconds(4f); // Duración real de la muerte
        IsDiying = false;
        UpdateAnimationState();
    }
    private IEnumerator ResetAfterDash()
    {
        yield return new WaitForSeconds(0.4f); // Duración del dash
        IsDashing = false;
        UpdateAnimationState();
    }

}
