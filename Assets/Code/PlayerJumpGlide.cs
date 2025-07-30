using System.Collections;
using UnityEngine;

public class PlayerJumpGlide : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("<size=15><color=#008B8B>Jump</color></size>")]
    [SerializeField] private AnimationCurve jumpForceCurve = AnimationCurve.Linear(0, 8, 1, 0);
    [SerializeField] private float jumpTime;
    [SerializeField] private float maxFallingSpeed;
    [Space]
    [Header("<size=15><color=#008B8B>Layers</color></size>")]
    [SerializeField] private LayerMask jumpableGround;
    
    [SerializeField] private LayerMask terrain;
    
    [Space]
    [Header("<size=15><color=#008B8B>BoxCast</color></size>")]
    [SerializeField] private float center;
    [SerializeField] private float size;
    [SerializeField] private float distance;

    [SerializeField] private Transform LeftRayPoint;
    [SerializeField] private Transform RightRayPoint;
    [SerializeField] private float rayDistance;

    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private float buttonPressedTime;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool coyoteTimeEnded;

    [HideInInspector] public bool canGlide = false;
    [HideInInspector] public bool isJumping;
    private bool glideOutputAffecting;

    [HideInInspector] public static bool isGlidingHard;
    [HideInInspector] public static bool isGlidingNormally;
    private bool glideSound = true;
    private float gravityGlide = -1.3f;
    private bool doGlide;
    private float drag = 25;

    private GameObject breakablePlatformObject;

    [HideInInspector] public bool inVerticalAirStream;

    [HideInInspector] public int numberOfKeys = 0;

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isJumping = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpOutput = Input.GetButtonUp("Jump");

        //RaycastHit2D leftRaycast = Physics2D.Raycast(LeftRayPoint.position, Vector2.up, rayDistance, terrain);
        //RaycastHit2D rightRaycast = Physics2D.Raycast(RightRayPoint.position, Vector2.up, rayDistance, terrain);
        //Debug.DrawRay(LeftRayPoint.position, Vector2.up * rayDistance);
        //Debug.DrawRay(RightRayPoint.position, Vector2.up * rayDistance);
        
        
            if (GetComponent<PlayerMovement>().controlsEnable)
            {
                if (jumpInput)
                {
                    jumpBufferCounter = jumpBufferTime;
                }
                else
                {
                    jumpBufferCounter -= Time.deltaTime;
                }
                if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
                {
                    if (!isJumping)
                    {
                        isJumping = true;
                        jumpBufferCounter = 0f; // Consumimos el buffer
                    }
                }



                if (isJumping)
                    {

                        buttonPressedTime += Time.deltaTime;
                        Jump();


                        if (buttonPressedTime >= jumpTime)
                        {

                            isJumping = false;
                        }
                        //if (leftRaycast && !rightRaycast)
                        //{
                        //    Debug.DrawRay(LeftRayPoint.position, Vector2.up * rayDistance, Color.red);
                        //    transform.position += new Vector3(1f, 0);
                        //}
                        //else if (rightRaycast && !leftRaycast)
                        //{
                        //    Debug.DrawRay(RightRayPoint.position, Vector2.up * rayDistance, Color.red);
                        //    transform.position -= new Vector3(1f, 0);
                        //}
                    }
                    else
                    {
                        buttonPressedTime = 0;
                    }

                    if (coyoteTimeCounter > 0)
                    {
                        coyoteTimeEnded = false;
                    }
                    else
                    {
                        coyoteTimeEnded = true;
                    }

                    if (isFalling())
                    {

                        if (rb.velocity.y < -30)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, -maxFallingSpeed);
                        }
                    }

                    if (jumpOutput)
                    {

                        isJumping = false;

                        coyoteTimeCounter = 0f;

                    
                    }

                    if (isGrounded())
                    {
                        drag = 20;
                        gravityGlide = -1.3f;
                        rb.drag = 0;

                        coyoteTimeCounter = coyoteTime;

                    

                        canGlide = false;
                    }

                    else
                    {
                        coyoteTimeCounter -= 2 * Time.deltaTime;
                    }
                }
        
        
    }
    private void FixedUpdate()
    {
        
    }

    private void Jump()
    {
        
            rb.velocity = new Vector2(rb.velocity.x, jumpForceCurve.Evaluate(Mathf.Clamp01(buttonPressedTime / jumpTime)));
        
        
    }

    
    private bool isFalling()
    {
        if (!isGrounded())
        {
            return rb.velocity.y < -.1f;
        }
        else
        {
            return false;
        }
    }

    public bool isGrounded()
    {
        return BoxCastDrawer.BoxCastAndDraw(new Vector2(coll.bounds.center.x, coll.bounds.center.y - center), new Vector2(coll.bounds.size.x - 0.1f, coll.bounds.size.y - size), 0f, Vector2.down, distance, jumpableGround);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BreakablePlatform"))
        {
            breakablePlatformObject = collision.gameObject;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (isGrounded())
            {
                rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
            }
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("MovingPlatform"))
    //    {
    //        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    //    }
    //}
}
