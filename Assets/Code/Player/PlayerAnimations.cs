using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    
    private enum MovementState { idle, running, jumping, falling, death, meleeAttacking };
    private PlayerMovement playerMovement;
    [HideInInspector] public bool inDialog;
    [HideInInspector] public bool playerLookingLeft;
    public bool IsAttacking { get; private set; } = false;
    public bool IsDiying { get; private set; } = false;


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
        if (IsDiying)
        {
            // No cambiar de estado mientras el jugador est� muriendo
            animator.SetInteger("State", (int)MovementState.death);
            return;
        }
        MovementState state;
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
        print($"Current Animation State: {state}");
    }
    public void TriggerMeleeAttackAnimation()
    {
        IsAttacking = true;
        animator.SetInteger("State", (int)MovementState.meleeAttacking);
        StartCoroutine(ResetAfterAttack());
    }
    public void TriggerDeathAnimation()
    {
        IsDiying = true;
        animator.SetInteger("State", (int)MovementState.death);
        StartCoroutine(ResetAfterDeath());
    }

    private IEnumerator ResetAfterAttack()
    {
        yield return new WaitForSeconds(0.5f); // Duraci�n real del ataque
        IsAttacking = false;
        UpdateAnimationState();
    }
    private IEnumerator ResetAfterDeath()
    {
        yield return new WaitForSeconds(3.8f); // Duraci�n real de la muerte
        IsDiying = false;
        UpdateAnimationState();
    }

}
