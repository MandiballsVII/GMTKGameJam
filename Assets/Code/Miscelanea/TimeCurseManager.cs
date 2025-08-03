using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCurseManager : MonoBehaviour
{
    [Header("<size=15><color=#008B8B>Time</color></size>")]
    public float totalTime = 120f; // en segundos
    private float currentTime;

    [Header("<size=15><color=#008B8B>References</color></size>")]
    public SpawnZone spawnZone; // Asignar desde el editor
    public Transform respawnPoint;
    public PlayerLife playerLife;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI loopCounterText;
    private int loopCounter = 0;

    private bool isTimerRunning = false;
    private bool hasLeftSafeZone = false;

    IEnumerator ResetTimerFromDeath()
    {
        currentTime = totalTime + 1f;
        yield return new WaitForSeconds(0.1f); // o el tiempo que uses
        isTimerRunning = false;
        hasLeftSafeZone = false;
        
    }
    void Start()
    {
        currentTime = totalTime;
        UpdateTimerUI();
        loopCounterText.text = "Loops: " + loopCounter.ToString();
    }

    void Update()
    {
        if (!hasLeftSafeZone && !spawnZone.IsPlayerInside)
        {
            hasLeftSafeZone = true;
            isTimerRunning = true;
        }

        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0f)
            {
                RespawnAndReset();
            }
        }
    }

    public void RespawnAndReset()
    {
        // Solo ejecutado desde el TimeCurseManager cuando el tiempo llega a 0
        playerLife.RespawnFromTime();
        //ResetTimerState();
    }

    public void ResetTimerState()
    {
        loopCounter++;
        loopCounterText.text = "Loops: " +  loopCounter.ToString();
        isTimerRunning = false;
        hasLeftSafeZone = false;
        currentTime = totalTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (currentTime > 0f)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            timerText.text = "00:00";
        }
    }
    public void ResetHasLeftZone()
    {
        StartCoroutine(ResetTimerFromDeath());
    }
}

