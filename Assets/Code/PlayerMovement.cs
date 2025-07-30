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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerJumpGlide = GetComponent<PlayerJumpGlide>();
        EnableControls();
    }

    private void Update()
    {
        
        if (controlsEnable)
            {
            if (Input.GetAxisRaw("Horizontal") <= -1 || Input.GetAxisRaw("Horizontal") >= 1)
                {
                    dirX = Input.GetAxisRaw("Horizontal");
                }
                else
                {
                    dirX = 0;
                }
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

    public void SetExternalVelocity(Vector2 velocity)
    {
        extraVelocity = velocity;
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
