using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Shoot</color></size>")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Vector2 shootDirection = Vector2.right; // Dirección editable
    public float shootForce = 5f;
    public float fireRate = 2f;

    private float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            FireArrow();
            fireTimer = 0f;
        }
    }

    void FireArrow()
    {
        if (arrowPrefab == null || shootPoint == null) return;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + 180f;

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle));

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootDirection.normalized * shootForce;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + (Vector3)(shootDirection.normalized * 1.5f));
        }
    }
#endif
}

