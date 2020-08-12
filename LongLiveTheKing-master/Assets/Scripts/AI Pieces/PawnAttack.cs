using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAttack : MonoBehaviour
{
    public float attackRate;
    public float range;
    public uint attackPower;
    private float nextAttack;
    private PlayerHealth playerHealth;

    void Start()
    {
        nextAttack = Time.time;
        attackPower = 1;
        range = GetComponent<SphereCollider>().radius;
    }

    void Update()
    {
        if (Time.time >= nextAttack)
        {
            if (playerHealth != null)
            {
                if(Vector3.Distance(transform.position, playerHealth.transform.position) <= range)
                {
                    playerHealth.DecreaseHealth(attackPower);
                    nextAttack = Time.time + attackRate;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (GetComponent<FlockAgent>().flock.State() == Flock.AIState.ATTACKING && other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }
}

