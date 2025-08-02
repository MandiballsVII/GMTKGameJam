using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostConeAttack : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Frost cone settings</color></size>")]
    public float coneAngle = 45f;
    public float coneLength = 5f;
    public float freezeDuration = 2f;
    public LayerMask affectedLayers;

    [Space]
    [Header("<size=15><color=#008B8B>Cooldown</color></size>")]
    public float cooldown = 1f;
    private bool isOnCooldown = false;

    [Space]
    [Header("<size=15><color=#008B8B>Frost particles</color></size>")]
    public ParticleSystem frostParticlesPrefab;

    private Vector2 lastDirection = Vector2.right;

    void Update()
    {
        Vector2 attackDirection = GetDiscreteDirection();

        if (Input.GetButtonDown("Fire4") && !isOnCooldown)
        {
            PerformFrostCone(lastDirection);
            StartCoroutine(CooldownRoutine());
        }
    }

    void OnEnable()
    {
        SetIconAlpha(1f);
    }

    Vector2 GetDiscreteDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 rawInput = new Vector2(x, y);

        if (rawInput.sqrMagnitude < 0.1f)
            return lastDirection != Vector2.zero ? lastDirection : Vector2.right;

        rawInput.Normalize();

        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            new Vector2(1, 1).normalized,
            Vector2.right,
            new Vector2(1, -1).normalized,
            Vector2.down,
            new Vector2(-1, -1).normalized,
            Vector2.left,
            new Vector2(-1, 1).normalized
        };

        float bestDot = -1f;
        Vector2 bestDir = Vector2.right;

        foreach (var dir in directions)
        {
            float dot = Vector2.Dot(rawInput, dir);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestDir = dir;
            }
        }

        lastDirection = bestDir;
        return bestDir;
    }

    void PerformFrostCone(Vector2 direction)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, coneLength, affectedLayers);

        foreach (Collider2D hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
            float angle = Vector2.Angle(direction, toTarget);

            if (angle <= coneAngle / 2f)
            {
                if (hit.TryGetComponent<IFreezable>(out var freezable))
                {
                    freezable.Freeze(freezeDuration);
                }
            }
        }

        if (frostParticlesPrefab != null)
        {
            var particles = Instantiate(frostParticlesPrefab, transform.position, Quaternion.identity);
            particles.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            Destroy(particles.gameObject, 2f);
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        SetIconAlpha(0.45f);

        yield return new WaitForSeconds(cooldown);

        SetIconAlpha(1f);
        isOnCooldown = false;
    }

    private void SetIconAlpha(float alpha)
    {
        if (PlayerHUD.instance != null && PlayerHUD.instance.frostConeIcon != null)
        {
            var color = PlayerHUD.instance.frostConeIcon.color;
            color.a = alpha;
            PlayerHUD.instance.frostConeIcon.color = color;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 origin = transform.position;
        Vector3 direction = (Vector3)lastDirection.normalized;

        float halfAngle = coneAngle / 2f;
        Quaternion leftRot = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRot = Quaternion.Euler(0, 0, halfAngle);

        Vector3 leftDir = leftRot * direction * coneLength;
        Vector3 rightDir = rightRot * direction * coneLength;

        Gizmos.DrawLine(origin, origin + leftDir);
        Gizmos.DrawLine(origin, origin + rightDir);
        Gizmos.DrawWireSphere(origin, coneLength);
    }
#endif
}
