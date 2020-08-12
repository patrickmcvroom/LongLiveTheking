using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookAnimations : MonoBehaviour
{
    private RookController rookController;

    void Start()
    {
        rookController = GetComponentInParent<RookController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rookController != null)
        {
            Debug.Log("NICK");
            if (rookController.State() == RookController.AIState.CHASING)
            {
                Debug.Log("Chasing State From Function");
                GetComponent<Animator>().SetBool("IsChasing", true);
                GetComponent<Animator>().SetBool("IsAttacking", false);
                GetComponent<Animator>().SetBool("IsChasing", false);
                GetComponent<Animator>().SetBool("IsChasing", true);
            }
           

            if (rookController.State() == RookController.AIState.ATTACKING)
            {
                Debug.Log("Attacking State From Function");
                GetComponent<Animator>().SetBool("IsAttacking", true);
                GetComponent<Animator>().SetBool("IsChasing", false);
            }
           
        }
    }
}