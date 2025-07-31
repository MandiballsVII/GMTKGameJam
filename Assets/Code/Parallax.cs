using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    //THIS SCRIT MUST BE ATTACHED TO ANY BACKGROUND IN THE LEVEL, THE BACKGROUND MUST BE IN THE POSITION IN WICH DARBY IS GONNA BE WHEN THE PARALLAX ACTIVATES
    [SerializeField] Camera cam;
    [SerializeField] Transform player;

    private Vector2 startPosition;
    float startZ;

    private Vector2 cameraTravel => (Vector2)cam.transform.position - startPosition;

    private float distanceFromPlayer => transform.position.z - player.position.z;

    private float clippingPlane => (cam.transform.position.z + (distanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane));

    private float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;
    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }
    public void Update()
    {
        //if (cam.GetComponent<PlayerCam>().enabled)
        //{
        //    Vector2 newPos = startPosition + cameraTravel * parallaxFactor;
        //    transform.position = new Vector3(newPos.x, newPos.y, startZ);
        //}
        
    }
}
