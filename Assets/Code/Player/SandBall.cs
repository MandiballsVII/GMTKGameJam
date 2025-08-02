using UnityEngine;

public class SandBall : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Lifetime</color></size>")]
    [SerializeField] private float lifetime = 3f;
    [Space]
    [Header("<size=15><color=#008B8B>Particles</color></size>")]
    [SerializeField] private GameObject hitEffect; // partículas o animación de destrucción

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Enemy"))
        {
            // Aplicar daño al enemigo
            // collision.GetComponent<Enemy>().TakeDamage(damage);
            collision.GetComponent<BasicEnemy>().Die();
            DestroySelf();
        }
        else if (collision.CompareTag("BreakableWall"))
        {
            // Romper la pared
            collision.GetComponent<BreakableBlocks>().DestroyWall();

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
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        Destroy(gameObject);
    }
}

