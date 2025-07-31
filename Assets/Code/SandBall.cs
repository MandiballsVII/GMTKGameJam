using UnityEngine;

public class Sandball : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject hitEffect; // partículas o animación de destrucción

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Aplicar daño al enemigo
            // collision.GetComponent<Enemy>().TakeDamage(damage);

            DestroySelf();
        }
        else if (collision.CompareTag("BreakableWall"))
        {
            // Romper la pared
            // collision.GetComponent<BreakableWall>().Break();

            DestroySelf();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

