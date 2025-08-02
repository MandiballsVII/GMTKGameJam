using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    GameObject[] wallBlocks;

    private void Start()
    {
        List<GameObject> foundBlocks = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.name.ToLower().Contains("block"))
            {
                foundBlocks.Add(child.gameObject);
            }
        }

        wallBlocks = foundBlocks.ToArray();
    }
    public void Break()
    {
        foreach (GameObject block in wallBlocks)
        {
            if (block != null)
            {
                block.GetComponent<Animator>().SetTrigger("Break");
                block.GetComponent<Collider2D>().enabled = false; // Desactivar colisionador
            }
        }
    }
}
