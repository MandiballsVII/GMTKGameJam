using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandballShooter : MonoBehaviour
{
    [Header("Proyectil")]
    [SerializeField] private GameObject sandballPrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireCooldown = 0.5f;

    private float lastFireTime;
    private Vector2 lastInputDirection = Vector2.right; // valor por defecto

    void Update()
    {
        Vector2 inputDir = GetInputDirection();

        if (inputDir != Vector2.zero)
        {
            lastInputDirection = inputDir.normalized;
        }

        if (Input.GetButtonDown("Fire2") && Time.time >= lastFireTime + fireCooldown)
        {
            LaunchSandball(lastInputDirection);
            lastFireTime = Time.time;
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
        GameObject projectile = Instantiate(sandballPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
    }
}


