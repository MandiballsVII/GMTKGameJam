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
        //hud.GetComponent<CanvasGroup>().alpha = 0f;
        //showHud();
    }

    void Update()
    {
        updateAnimationState();
    }

    private void updateAnimationState()
    {
        MovementState state;
        int idleNumber;

        if (playerMovement.dirX > 0f)
        {
            //hideHud();
            state = MovementState.running;
            if (transform.rotation == new Quaternion(0, -1, 0, 0))
            {
                playerLookingLeft = false;
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                //transform.Rotate(0, 180, 0);
            }            
            //sprite.flipX = false;
            idleNumber = 0;
            animator.SetInteger("IdleNumber", idleNumber);
        }
        else if (playerMovement.dirX < 0f)
        {
            //hideHud();
            state = MovementState.running;
            if (transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                playerLookingLeft = true;
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                //transform.Rotate(0, 180, 0);
            }
            //sprite.flipX = true;
            idleNumber = 0;
            animator.SetInteger("IdleNumber", idleNumber);
        }
        else
        {
            //showHud();
            idleNumber = UnityEngine.Random.Range(0, 3);
            animator.SetInteger("IdleNumber", idleNumber);
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f && !gameObject.GetComponent<PlayerJumpGlide>().isGrounded())
        {
            //hideHud();
            animator.StopPlayback();
            state = MovementState.jumping;
            idleNumber = 0;
            animator.SetInteger("IdleNumber", idleNumber);
        }

        else if (rb.velocity.y < -0.1f)
        {
            //hideHud();
            idleNumber = 0;
            animator.SetInteger("IdleNumber", idleNumber);            
            state = MovementState.falling; 

        }
        if(inDialog)
        {
            state = MovementState.idle;
            idleNumber = 0;
            animator.SetInteger("IdleNumber", idleNumber);
        }
        
        animator.SetInteger("state", (int)state);
    }

    
}
