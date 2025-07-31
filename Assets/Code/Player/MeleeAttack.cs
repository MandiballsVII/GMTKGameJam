using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Rango de ataque</color></size>")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackOffsetY = 0f;
    [Space]
    [Header("<size=15><color=#008B8B>Tama�o de ataque</color></size>")]
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1f, 1f);
    [Space]
    [Header("<size=15><color=#008B8B>Cooldown de ataque</color></size>")]
    [SerializeField] private float attackCooldown = 0.5f;
    [Space]
    [Header("<size=15><color=#008B8B>Capas de enemigos</color></size>")]
    [SerializeField] private LayerMask enemyLayers;

    private float lastAttackTime = -Mathf.Infinity;
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    private void PerformAttack()
    {
        // Posici�n del ataque, justo frente al personaje
        Vector2 attackDirection = new Vector2(movement.lastFacingDirection, 0);
        Vector2 attackOrigin = (Vector2)transform.position + new Vector2(attackDirection.x * attackRange, attackOffsetY);

        // Detecta enemigos en el �rea del ataque
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackOrigin, attackBoxSize, 0f, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            // Aqu� puedes enviar da�o al enemigo
            Debug.Log("Golpeaste a " + enemy.name);
            // enemy.GetComponent<Enemy>().TakeDamage(damage); // si tienes algo as�
        }

        // (Opcional) aqu� podr�as reproducir una animaci�n o sonido
    }

    // Dibuja el �rea de ataque en el editor
    private void OnDrawGizmosSelected()
    {
        if (movement == null) return;

        Vector2 attackDirection = new Vector2(movement.lastFacingDirection, 0);
        Vector2 attackOrigin = (Vector2)transform.position + new Vector2(attackDirection.x * attackRange, attackOffsetY);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin, attackBoxSize);
    }
}

