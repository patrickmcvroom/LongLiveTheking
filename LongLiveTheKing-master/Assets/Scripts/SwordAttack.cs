using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    //public bool isAttacking = false;

    void OnTriggerEnter(Collider other)
    {
        if (animator.GetBool("IsAttacking") && other.CompareTag("Enemy"))
        {
            FlockAgent otherAgent = other.GetComponent<FlockAgent>();
            if (otherAgent != null)
            {
                otherAgent.Die();
                return;
            }
            RookController rookController = other.GetComponent<RookController>();
            if (rookController != null)
            {
                rookController.Die();
                return;
            }
        }
    }
}
