using UnityEngine;

public class Sandball : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject hitEffect; // part�culas o animaci�n de destrucci�n

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Aplicar da�o al enemigo
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

