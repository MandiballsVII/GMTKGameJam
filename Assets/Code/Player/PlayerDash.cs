using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerMovement movement;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer;
    private Image dashIcon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }
    void OnEnable()
    {
        if (PlayerHUD.instance != null && PlayerHUD.instance.dashIcon != null)
        {
            dashIcon = PlayerHUD.instance.dashIcon;
            SetIconAlpha(1f); // Hacer visible al activarse
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire3") && canDash && !isDashing)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            int dashDirX = 0;

            if (inputX != 0)
            {
                dashDirX = inputX > 0 ? 1 : -1;
            }
            else
            {
                dashDirX = movement.lastFacingDirection;
            }

            Vector2 inputDirection = new Vector2(dashDirX, 0f);

            StartCoroutine(PerformDash(inputDirection.normalized));
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;
        dashTimer = 0f;

        movement.DisableControls();

        rb.velocity = Vector2.zero;

        SetIconAlpha(0.45f);

        while (dashTimer < dashDuration)
        {
            rb.velocity = direction * dashForce;
            dashTimer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(0, rb.velocity.y);
        movement.EnableControls();
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

        SetIconAlpha(1f);
    }
    private void SetIconAlpha(float alpha)
    {
        if (dashIcon == null) return;

        var color = dashIcon.color;
        color.a = alpha;
        dashIcon.color = color;
    }
}
