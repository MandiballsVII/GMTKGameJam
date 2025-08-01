using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    GameObject respawnPoint;
    public TimeCurseManager timeManager;
    private bool isDead = false;

    private void Awake()
    {
        
    }
    void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
    }

    void Update()
    {
        
    }

    public void RespawnFromDeath()
    {
        if (isDead) return;
        isDead = true;

        transform.position = respawnPoint.transform.position;

        Debug.Log("Jugador ha sido re-spawneado por muerte en " + respawnPoint);

        if (timeManager != null)
            timeManager.ResetTimerState();
            timeManager.ResetHasLeftZone();

        StartCoroutine(ReviveDelay());
    }
    public void RespawnFromTime()
    {
        if (isDead) return;
        isDead = true;

        transform.position = respawnPoint.transform.position;

        Debug.Log("Jugador ha sido re-spawneado por tiempo agotado en " + respawnPoint);

        StartCoroutine(ReviveDelay());
    }
    private IEnumerator ReviveDelay()
    {
        yield return new WaitForSeconds(0.5f); // o el tiempo que uses
        isDead = false;
    }
}
