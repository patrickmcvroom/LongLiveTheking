using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    private Animator animator;
    private PlayerHealth playerHealth;

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.currentHealth == 0)
        {
            animator.SetBool("IsDead", true);
        }
    }
}
