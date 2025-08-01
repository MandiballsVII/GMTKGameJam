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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Aquí puedes implementar la lógica de daño o muerte del jugador
            Debug.Log("Jugador ha chocado con un enemigo");
            Respawn();
        }
    }
    
    public void Respawn()
    {
        // Mueve al jugador a la posición de respawn
        transform.position = respawnPoint.transform.position;
        // Aquí puedes implementar lógica adicional como restablecer la salud del jugador, animaciones, etc.
        Debug.Log("Jugador ha sido re-spawneado en " + respawnPoint);
    }
}
