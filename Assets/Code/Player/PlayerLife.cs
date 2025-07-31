using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    Vector2 respawnPoint;

    private void Awake()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Aqu� puedes implementar la l�gica de da�o o muerte del jugador
            Debug.Log("Jugador ha chocado con un enemigo");
            Respawn();
        }
    }
    
    public void Respawn()
    {
        // Mueve al jugador a la posici�n de respawn
        transform.position = respawnPoint;
        // Aqu� puedes implementar l�gica adicional como restablecer la salud del jugador, animaciones, etc.
        Debug.Log("Jugador ha sido re-spawneado en " + respawnPoint);
    }
}
