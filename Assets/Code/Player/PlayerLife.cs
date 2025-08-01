using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    GameObject respawnPoint;

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

    
    
    public void Respawn()
    {
        // Mueve al jugador a la posición de respawn
        transform.position = respawnPoint.transform.position;
        // Aquí puedes implementar lógica adicional como restablecer la salud del jugador, animaciones, etc.
        Debug.Log("Jugador ha sido re-spawneado en " + respawnPoint);
    }
}
