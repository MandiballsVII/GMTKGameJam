using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject[] enemyList;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        BasicEnemy.OnEnemyDied += OnEnemyDied;
    }
    private void OnDisable()
    {
        BasicEnemy.OnEnemyDied -= OnEnemyDied;
    }

    void Update()
    {
        
    }
    private void OnEnemyDied(BasicEnemy enemy)
    {
        // Si el enemigo está en la lista
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] == enemy.gameObject)
            {
                // Marcamos como "muerto"
                enemyList[i] = null;
                break;
            }
        }

        // Revisar si todos están muertos
        if (AllEnemiesDead())
        {
            OpenDoor();
        }
    }
    private bool AllEnemiesDead()
    {
        foreach (var enemy in enemyList)
        {
            if (enemy != null && enemy.activeInHierarchy)
                return false;
        }
        return true;
    }

    private void OpenDoor()
    {
        Debug.Log("¡Todos los enemigos han muerto! Puerta abierta.");
        // Aquí puedes activar animación, colisionador, efecto, etc.
        gameObject.SetActive(false); // o anim.SetTrigger("Open");
    }
}
