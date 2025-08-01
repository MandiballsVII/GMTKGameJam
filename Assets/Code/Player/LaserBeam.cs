using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Laser Settings</color></size>")]
    public Transform laserOrigin;
    public float laserLength = 10f;
    public float duration = 1f;
    public float cooldown = 2f;
    public LayerMask collisionMask;
    public LineRenderer lineRenderer;
    public GameObject impactParticlesPrefab;

    private float activeTime = 0f;
    private float cooldownTimer = 0f;
    private bool isFiring = false;

    private Vector2 lastDirection = Vector2.right;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        Vector2 direction = GetDiscreteInputDirection();
        if (direction != Vector2.zero)
            lastDirection = direction;

        if (Input.GetButtonDown("Fire5") && cooldownTimer <= 0f)
        {
            isFiring = true;
            activeTime = duration;
            cooldownTimer = cooldown;
        }

        if (isFiring)
        {
            activeTime -= Time.deltaTime;
            UpdateLaser(lastDirection);

            if (activeTime <= 0f)
            {
                isFiring = false;
                lineRenderer.enabled = false;
            }
        }
    }

    Vector2 GetDiscreteInputDirection()
    {
        int x = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int y = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        return new Vector2(x, y).normalized;
    }

    void UpdateLaser(Vector2 direction)
    {
        lineRenderer.enabled = true;

        Vector3 start = laserOrigin.position;
        RaycastHit2D hit = Physics2D.Raycast(start, direction, laserLength, collisionMask);

        Vector3 end;
        if (hit.collider != null)
        {
            end = hit.point;

            // Instanciar part�culas en el impacto
            if (impactParticlesPrefab != null)
            {
                Quaternion rot = Quaternion.LookRotation(Vector3.forward, direction); // orientaci�n hacia el rayo
                var particles = Instantiate(impactParticlesPrefab, end, rot);
                Destroy(particles.gameObject, 2f);
            }
        }
        else
        {
            end = start + (Vector3)(direction.normalized * laserLength);
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
