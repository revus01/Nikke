using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{


    public float health, maxHealth;
    public float offensePower, defensePower;

    bool isDead;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {

        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        isDead = false;
        healthBar.UpdateHealthbar(health, maxHealth);

    }

    public void TakeDamage(float damage)
    {
        Debug.Log("데미지 입음");
        if (!isDead)
        {
            health -= damage;
            healthBar.UpdateHealthbar(health, maxHealth);
            if (health <= 0)
            {
                isDead = true;
                Debug.Log("dead");
                Destroy(gameObject);

            }
        }

    }
}
