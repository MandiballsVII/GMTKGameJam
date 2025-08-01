using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    
    private enum MovementState { idle, running, jumping, falling, death, glidingHard };
    private PlayerMovement playerMovement;
    [HideInInspector] public bool inDialog;
    [HideInInspector] public bool playerLookingLeft;

    
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
    }

    
}
