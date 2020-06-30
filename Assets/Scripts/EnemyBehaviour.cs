﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int scoreToAdd = 10;
    [SerializeField] private int coinsToAdd = 5;
    private FireBullets playerObject;
    private bool isDead = false;

    private void Start()
    {
        playerObject = GameObject.FindObjectOfType<FireBullets>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            DealDamage(playerObject.DamageDealt);
            Destroy(collision.gameObject);
        }

        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            GameManager.Instance.AddCoins(coinsToAdd);
            GameManager.Instance.AddScore(scoreToAdd);

            switch (gameObject.tag)
            {
                case "Asteroid":
                    gameObject.GetComponent<AsteroidScript>().SpawnChildAsteroids();
                    break;

                default:
                    Destroy(gameObject);
                    break;
            }
        }
    }

    private void DealDamage(int damageDealt)
    {
        enemyHealth -= damageDealt;
    }
}