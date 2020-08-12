using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;
    PlayerHealth testing;

    public Animator weaponAnimator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("IsAttacking", Input.GetKey(KeyCode.V));
        weaponAnimator.SetBool("IsAttacking", Input.GetKey(KeyCode.V));
        animator.SetBool("IsBlocking", Input.GetKey(KeyCode.E));

        if(Input.GetKeyUp(KeyCode.M))
        {
            animator.SetBool("IsDead", true);

            
            var capsule = GetComponent<CapsuleCollider>();
            var sphere = transform.gameObject.AddComponent<SphereCollider>();
            sphere.center = capsule.center;
            sphere.radius = 0.2f;

            StartCoroutine(DisableCollider(capsule, 1.3f));
        }
    }

    private IEnumerator DisableCollider(CapsuleCollider capsule, float delay)
    {
        yield return new WaitForSeconds(delay);
        capsule.enabled = false;
    }
}
