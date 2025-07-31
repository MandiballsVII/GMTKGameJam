using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IFreezable
{
    private bool isFrozen = false;
    private float freezeTimer = 0f;

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        // Aquí puedes pausar animaciones, movimiento, etc.
        print("Enemy frozen for " + duration + " seconds.");
    }

    void Update()
    {
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0)
            {
                isFrozen = false;
                // Reactivar comportamiento normal
            }
        }
    }
}
