using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.2f;
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f); // look ahead y altura
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSmoothing = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private float lastTargetX;
    private float lookAheadDirX;
    private Vector3 targetPosition;

    private void Start()
    {
        if (target != null)
            lastTargetX = target.position.x;
        AudioManager.instance.PlayTheSafeZone();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float moveDeltaX = target.position.x - lastTargetX;

        // Determinar dirección de movimiento
        if (Mathf.Abs(moveDeltaX) > 0.01f)
        {
            lookAheadDirX = Mathf.Sign(moveDeltaX);
        }

        Vector3 lookAheadOffset = new Vector3(lookAheadDirX * lookAheadDistance, offset.y, -10f);
        targetPosition = new Vector3(target.position.x, target.position.y, 0) + lookAheadOffset;

        // Suaviza el movimiento
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, -10f); // -10 para que siga en la misma capa de cámara

        lastTargetX = target.position.x;
    }
}

