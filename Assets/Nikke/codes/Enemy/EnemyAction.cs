using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{


    public float health, maxHealth;
    public float offensePower, defensePower;

    public float moveSpeed = 5f;
    public float moveDistance = 10f; // 전진 후진 거리

    private bool movingForward = true;
    private Vector3 initialPosition;

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
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        if (movingForward)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // 이동 거리 체크 및 방향 전환
        float currentDistance = Vector3.Distance(initialPosition, transform.position);
        if (currentDistance >= moveDistance)
        {
            movingForward = !movingForward;
        }
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
