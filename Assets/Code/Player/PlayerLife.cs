using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    GameObject respawnPoint;
    public TimeCurseManager timeManager;
    [HideInInspector] public bool isDead = false;
    PlayerAnimations playerAnimations;
    Rigidbody2D rb;

    private void Awake()
    {
        
    }
    void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
        playerAnimations = GetComponent<PlayerAnimations>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PauseMenu.isPaused)
            return;

        if (Input.GetButtonDown("Restart"))
        {
            RespawnFromDeath();
        }
        if (isDead)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void RespawnFromDeath()
    {
        if (isDead) return;
        PerformDeathAnimation();
    }
    public void RespawnFromTime()
    {
        if (isDead) return;
        PerformDeathAnimation();
    }
    private IEnumerator ReviveDelay()
    {
        yield return new WaitForSeconds(0.5f); // o el tiempo que uses
        isDead = false;
    }
    void ResetSceneObjects()
    {
        IRepositionable[] resettables = FindObjectsOfType<MonoBehaviour>(true).OfType<IRepositionable>().ToArray();

        foreach (var obj in resettables)
        {
            obj.ResetToOrigin();
        }
    }

    public void PerformDeathAnimation()
    {
        if (isDead) return;
        isDead = true;

        // 1. Desactivar controles y colisiones
        GetComponent<PlayerMovement>().DisableControls();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // 2. Activar animación de muerte
        playerAnimations.TriggerDeathAnimation();

        // 3. Lanzar respawn tras terminar la animación
        StartCoroutine(RespawnAfterDeathAnimation());
    }
    private IEnumerator RespawnAfterDeathAnimation()
    {
        rb.bodyType = RigidbodyType2D.Static; // Desactivar física para evitar colisiones durante la animación
        float deathDuration = 4f; // igual que en PlayerAnimations
        yield return new WaitForSeconds(deathDuration);

        // Reposicionar jugador
        transform.position = respawnPoint.transform.position;

        // Restaurar estado
        GetComponent<PlayerMovement>().EnableControls();
        isDead = false;

        if (timeManager != null)
        {
            timeManager.ResetTimerState();
            timeManager.ResetHasLeftZone();
        }
        rb.bodyType = RigidbodyType2D.Dynamic; // Reactivar física
        Debug.Log("Jugador ha muerto y reaparecido en: " + respawnPoint);
        ResetSceneObjects();
    }
}
