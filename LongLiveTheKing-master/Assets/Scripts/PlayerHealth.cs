using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 100)]
    public uint maxHealth;
    public uint currentHealth;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 0)
        {
            PlayerDeathScene();
        }
    }

    private void PlayerDeathScene()
    {
        animator.SetBool("IsDead", true);


        var capsule = GetComponent<CapsuleCollider>();
        var sphere = transform.gameObject.AddComponent<SphereCollider>();
        sphere.center = capsule.center;
        sphere.radius = 0.2f;

        StartCoroutine(DisableCollider(capsule, 1.3f));
    }

    private IEnumerator DisableCollider(CapsuleCollider capsule, float delay)
    {
        yield return new WaitForSeconds(delay);
        capsule.enabled = false;
    }

    public void DecreaseHealth(uint amount)
    {
        if(currentHealth != 0)
        {
            currentHealth = (uint)Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        }
    }

    public void IncreaseHealth(uint amount)
    {
        if (currentHealth != maxHealth)
        {
            currentHealth = (uint)Mathf.Clamp(currentHealth + amount, 0, maxHealth);           
        }
    }
}
