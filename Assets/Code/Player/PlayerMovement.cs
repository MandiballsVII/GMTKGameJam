using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 56f;
    [HideInInspector] public float dirX = 0f;
    public bool controlsEnable = true;
    private Vector2 extraVelocity = Vector2.zero;
    private PlayerJumpGlide playerJumpGlide;
    private float timeToCancelExtraVelocity = 0.2f;
    private float timer = 0f;
    [HideInInspector] public int lastFacingDirection = 1; // 1 = derecha, -1 = izquierda
    private PlayerAnimations playerAnimations;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerJumpGlide = GetComponent<PlayerJumpGlide>();
        playerAnimations = GetComponent<PlayerAnimations>();
        EnableControls();
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
            return;
        if (controlsEnable)
        {
            bool isAttacking = playerAnimations != null && playerAnimations.IsAttacking;
            bool isGrounded = playerJumpGlide != null && playerJumpGlide.enabled && playerJumpGlide.isGrounded();

            if (isAttacking && isGrounded)
            {
                dirX = 0; // se bloquea el movimiento horizontal durante el ataque en el suelo
            }
            else
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");

                if (horizontalInput <= -1 || horizontalInput >= 1)
                {
                    dirX = horizontalInput;
                    lastFacingDirection = dirX > 0 ? 1 : -1;
                }
                else
                {
                    dirX = 0;
                }
            }
        }

        if (dirX > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(dirX < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void FixedUpdate()
    {        
        if(controlsEnable)
        {                
            Vector2 inputVelocity = new Vector2(dirX * moveSpeed * Time.deltaTime, rb.velocity.y);
            rb.velocity = inputVelocity + extraVelocity;  
        }            
    }

    public void EnableControls()
    {
        controlsEnable = true;
    }

    public void DisableControls()
    {
        controlsEnable = false;
    }
    
}
