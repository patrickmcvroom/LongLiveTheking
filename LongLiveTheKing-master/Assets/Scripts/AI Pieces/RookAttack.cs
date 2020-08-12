using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookAttack : MonoBehaviour
{
    public float attackRate;
    public float range;
    public uint attackPower;
    private float nextAttack;
    private PlayerHealth playerHealth;

    void Start()
    {
        nextAttack = Time.time;
        attackPower = 3;
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
        if (other.CompareTag("Player"))
        {
            if (GetComponent<RookController>().State() == RookController.AIState.ATTACKING)
            {
                Debug.Log("Health deducting");
                playerHealth = other.GetComponent<PlayerHealth>();
            }
        }
    }
}

