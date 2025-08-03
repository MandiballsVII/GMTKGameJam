using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IFreezable
{
    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    private Transform targetPatrolPoint;
    private GameObject player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isFrozen = false;
    private float freezeTimer = 0f;
    private float attackTimer = 0f;
    private float recoverTime = 4f;
    private float recoverTimer = 0f;
    private bool isAttacking = false;

    private Animator animator;

    private enum State {Ilde, Patrolling, Chasing, Attacking, Recovering, Diying }
    private State currentState = State.Patrolling;

    private Vector3 pointAPosition;
    private Vector3 pointBPosition;

    public static event Action<BasicEnemy> OnEnemyDied;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPatrolPoint = pointA;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        pointAPosition = pointA.position;
        pointBPosition = pointB.position;

        // Opcional: desactivar o destruir los objetos visuales
        pointA.gameObject.SetActive(false);
        pointB.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentState = State.Patrolling;
    }
    private void Update()
    {
        print($"Enemy State: {currentState}");
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
                Unfreeze();
            return;
        }

        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrolling:
                animator.SetInteger("State", (int)State.Patrolling);
                Patrol();
                DetectPlayer();
                break;
            case State.Chasing:
                animator.SetInteger("State", (int)State.Chasing);
                ChasePlayer();
                break;
            case State.Attacking:
                animator.SetInteger("State", (int)State.Attacking);
                if (!isAttacking)
                {
                    isAttacking = true;
                }
                //if (attackTimer <= 0f)
                //{
                //    currentState = State.Patrolling;
                //    isAttacking = false;
                //}
                break;
            case State.Recovering:
                recoverTimer += Time.deltaTime;
                if (recoverTimer >= recoverTime)
                {
                    recoverTimer = 0f;
                    currentState = State.Patrolling;
                }
                animator.SetInteger("State", (int)State.Recovering);
                
                break;
        }
    }

    private void Patrol()
    {
        Vector2 target = (targetPatrolPoint == pointA) ? pointAPosition : pointBPosition;
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(dir.x * patrolSpeed, rb.velocity.y);

        // Hacer que el enemigo mire hacia la dirección del movimiento
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Vector2.Distance(transform.position, target) < 0.2f)
            targetPatrolPoint = targetPatrolPoint == pointA ? pointB : pointA;
    }


    private void DetectPlayer()
    {
        if (player == null) return;

        float heightDifference = Mathf.Abs(player.transform.position.y - transform.position.y);
        float horizontalDistance = Mathf.Abs(player.transform.position.x - transform.position.x);

        if (heightDifference < 1f && horizontalDistance <= detectionRange)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                currentState = State.Chasing;
            }
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.transform.position, transform.position);

        if (dist > detectionRange)
        {
            currentState = State.Patrolling;
            return;
        }

        if (dist <= attackRange && attackTimer <= 0f)
        {
            Attack();
            return;
        }

        Vector2 dir = (player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * chaseSpeed, rb.velocity.y);

        // Mirar hacia el jugador
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Attack()
    {
        attackTimer = attackCooldown;
        currentState = State.Attacking;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

    }
    public void DealDamageIfInRange()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.transform.position, transform.position);
        if (dist <= attackRange)
        {
            player.GetComponent<PlayerLife>()?.RespawnFromDeath();
        }
    }
    public void EndAttack()
    {
        isAttacking = false;
        currentState = State.Recovering;
        rb.velocity = Vector2.zero; // Detener el movimiento al finalizar el ataque
        
    }

    //private IEnumerator AttackCooldown()
    //{
    //    yield return new WaitForSeconds(attackCooldown);
    //    currentState = State.Patrolling;
    //}

    public void Freeze(float duration)
    {
        if (isFrozen) return;

        isFrozen = true;
        freezeTimer = duration;

        rb.velocity = Vector2.zero;         // detener movimiento
        rb.isKinematic = true;              // que no lo afecte la física
        animator.speed = 0f;                // congelar animaciones
        sr.color = Color.blue;              // feedback visual
    }

    private void Unfreeze()
    {
        isFrozen = false;

        rb.isKinematic = false;             // volver a usar física
        animator.speed = 1f;                // reanudar animaciones
        sr.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFrozen && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerLife>()?.RespawnFromDeath();
        }
    }
    public void Die()
    {
        print("Enemy died: " + gameObject.name);
        // Tu lógica de muerte: animaciones, efectos, etc.
        OnEnemyDied?.Invoke(this); // lanza el evento
        currentState = State.Diying;
        animator.SetInteger("State", (int)State.Diying);
    }

    public void SetEnemyInactive()
    {
        gameObject.SetActive(false);

    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(pointA.position, new Vector3(0.5f, 2f, 1f));
            Gizmos.DrawWireCube(pointB.position, new Vector3(0.5f, 2f, 1f));
            return;
        }

        // Detección 360º: dibujo una esfera o caja centrada en el enemigo
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRange);

        // Ataque solo hacia adelante, según la orientación
        float facingDir = Mathf.Sign(transform.localScale.x);
        Vector3 attackCenter = transform.position + Vector3.right * facingDir * (attackRange / 2);
        Vector3 attackSize = new Vector3(attackRange, 1f, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackSize);
    }

}
