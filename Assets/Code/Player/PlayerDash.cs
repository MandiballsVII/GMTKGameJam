using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
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

        // Cancelar velocidad anterior
        rb.velocity = Vector2.zero;

        while (dashTimer < dashDuration)
        {
            rb.velocity = direction * dashForce;
            dashTimer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector2(0, rb.velocity.y); // Mantén vertical si saltando
        movement.EnableControls();
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
