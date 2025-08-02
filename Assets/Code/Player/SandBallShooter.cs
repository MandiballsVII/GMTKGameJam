using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandballShooter : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Proyectil</color></size>")]
    [SerializeField] private GameObject sandballPrefab;
    [Space]
    [Header("<size=15><color=#008B8B>Proyectile speed</color></size>")]
    [SerializeField] private float projectileSpeed = 10f;
    [Space]
    [Header("<size=15><color=#008B8B>Cooldown</color></size>")]
    [SerializeField] private float fireCooldown = 0.5f;
    [Space]
    [Header("<size=15><color=#008B8B>SandBall spawn point</color></size>")]
    [SerializeField] private Transform sandballSpawnPoint;

    private float lastFireTime;
    private Vector2 lastInputDirection = Vector2.right;

    private bool isOnCooldown = false;

    void OnEnable()
    {
        SetIconAlpha(1f);
    }

    void Update()
    {
        Vector2 inputDir = GetInputDirection();

        if (inputDir != Vector2.zero)
            lastInputDirection = inputDir.normalized;

        if (Input.GetButtonDown("Fire2") && !isOnCooldown)
        {
            LaunchSandball(lastInputDirection);
            StartCoroutine(CooldownRoutine());
        }
    }

    private Vector2 GetInputDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        return new Vector2(x, y);
    }

    private void LaunchSandball(Vector2 direction)
    {
        GameObject projectile = Instantiate(sandballPrefab, sandballSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        SetIconAlpha(0.45f);

        yield return new WaitForSeconds(fireCooldown);

        SetIconAlpha(1f);
        isOnCooldown = false;
    }

    private void SetIconAlpha(float alpha)
    {
        if (PlayerHUD.instance != null && PlayerHUD.instance.sandBallIcon != null)
        {
            var color = PlayerHUD.instance.sandBallIcon.color;
            color.a = alpha;
            PlayerHUD.instance.sandBallIcon.color = color;
        }
    }
}
