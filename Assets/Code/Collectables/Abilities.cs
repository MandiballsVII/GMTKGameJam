using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] private abilityName ability;
    private GameObject player;

    // Variables para el efecto de flotación
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatFrequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ability == abilityName.Jump)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<PlayerJumpGlide>().enabled = true;
                Destroy(gameObject);
            }
            else if (ability == abilityName.Dash)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<PlayerDash>().enabled = true;
                Destroy(gameObject);
            }
            else if (ability == abilityName.MeleeAtack)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<MeleeAttack>().enabled = true;
                Destroy(gameObject);
            }
            else if (ability == abilityName.SandBall)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<SandballShooter>().enabled = true;
                Destroy(gameObject);
            }
            else if (ability == abilityName.FrostCone)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<FrostConeAttack>().enabled = true;
                Destroy(gameObject);
            }
            else if (ability == abilityName.LaserBeam)
            {
                print("Ability " + ability + " acquired!");
                player.GetComponent<LaserBeam>().enabled = true;
                Destroy(gameObject);
            }
            AudioManager.instance.PlaySFX(AudioManager.instance.cogerItem);
        }
    }
}

public enum abilityName { Jump, Dash, MeleeAtack, SandBall, FrostCone, LaserBeam };
