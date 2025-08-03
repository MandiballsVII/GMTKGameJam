using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BreakableBlocks : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        animator.Play("Idle");
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void DestroyWall()
    {
        gameObject.GetComponentInParent<BreakableWall>().Break();

    }
}
