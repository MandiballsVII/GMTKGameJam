using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour, IRepositionable
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetToOrigin()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        gameObject.SetActive(true);
    }
}

