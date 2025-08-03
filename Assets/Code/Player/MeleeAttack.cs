using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Rango de ataque</color></size>")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackOffsetY = 0f;

    [Space]
    [Header("<size=15><color=#008B8B>Tamaño de ataque</color></size>")]
    [SerializeField] private Vector2 attackBoxSize = new Vector2(1f, 1f);

    [Space]
    [Header("<size=15><color=#008B8B>Cooldown de ataque</color></size>")]
    [SerializeField] private float attackCooldown = 0.5f;

    [Space]
    [Header("<size=15><color=#008B8B>Capas de enemigos</color></size>")]
    [SerializeField] private LayerMask enemyLayers;

    private bool isOnCooldown = false;
    private PlayerMovement movement;
    private PlayerAnimations playerAnimations;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void OnEnable()
    {
        SetIconAlpha(1f);
    }

    private void Update()
    {
        if (PauseMenu.isPaused)
            return;
        if (Input.GetButtonDown("Fire1") && !isOnCooldown)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {  
        if (playerAnimations != null)
            playerAnimations.TriggerMeleeAttackAnimation();
        StartCoroutine(CooldownRoutine());
        //TODO
        // Poner una animación o efecto sonoro
    }
    public void DetectHit()
    {
        Vector2 attackDirection = new Vector2(movement.lastFacingDirection, 0);
        Vector2 attackOrigin = (Vector2)transform.position + new Vector2(attackDirection.x * attackRange, attackOffsetY);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackOrigin, attackBoxSize, 0f, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            Debug.Log("Golpeaste a " + enemy.name);
            if (enemy.TryGetComponent<BasicEnemy>(out var basicEnemy))
            {
                if (!basicEnemy.isFrozen)
                {
                    basicEnemy.Die();
                }
            }
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        SetIconAlpha(0.45f);

        yield return new WaitForSeconds(attackCooldown);

        SetIconAlpha(1f);
        isOnCooldown = false;
    }

    private void SetIconAlpha(float alpha)
    {
        if (PlayerHUD.instance != null && PlayerHUD.instance.meleeAttackIcon != null)
        {
            var color = PlayerHUD.instance.meleeAttackIcon.color;
            color.a = alpha;
            PlayerHUD.instance.meleeAttackIcon.color = color;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (movement == null) return;

        Vector2 attackDirection = new Vector2(movement.lastFacingDirection, 0);
        Vector2 attackOrigin = (Vector2)transform.position + new Vector2(attackDirection.x * attackRange, attackOffsetY);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin, attackBoxSize);
    }
}
