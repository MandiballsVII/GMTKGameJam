using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour, IFreezable
{
    [Header("<size=15><color=#008B8B>Lifetime</color></size>")]
    [SerializeField] private float lifetime = 5f;

    [Space]
    [Header("<size=15><color=#008B8B>Damage</color></size>")]
    public float damage = 1f;

    private Rigidbody2D rb;
    private Vector2 storedVelocity;
    private bool isFrozen = false;
    private int originalLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalLayer = gameObject.layer; // Enemy = 7
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFrozen) return; // No hacer daño si está congelada

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player hit by arrow");
            collision.collider.GetComponent<PlayerLife>()?.RespawnFromDeath();
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            // Si la flecha colisiona con el terreno, se destruye
            Destroy(gameObject);
        }
    }

    public void Freeze(float duration)
    {
        if (isFrozen) return;

        isFrozen = true;

        storedVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        GetComponent<SpriteRenderer>().color = Color.blue;
        gameObject.layer = 6; // Terrain (para poder pisarla)

        Debug.Log("Arrow frozen for " + duration + " seconds.");
        Invoke(nameof(Unfreeze), duration);
    }

    private void Unfreeze()
    {
        isFrozen = false;

        rb.isKinematic = false;
        rb.velocity = storedVelocity;

        GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.layer = originalLayer; // Enemy = 7

        Debug.Log("Arrow unfrozen.");
    }
}
