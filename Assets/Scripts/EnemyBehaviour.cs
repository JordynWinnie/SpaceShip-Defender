﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] float enemyHealth = 100;
    [SerializeField] private int scoreToAdd = 10;
    [SerializeField] private int coinsToAdd = 5;
    [SerializeField] int damageDealt = 1;
    private FireBullets playerObject;
    private bool isDead = false;

    private void Start()
    {
        this.enemyHealth *= (SpawningManagement.Factor + 1);
        playerObject = FindObjectOfType<FireBullets>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.instance.PlayExplosionAnimation(collision.transform, false);
            DealDamage(playerObject.damageDealt);
            Destroy(collision.gameObject);
        }

        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            GameManager.instance.PlayExplosionAnimation(collision.transform, true);
            GameManager.instance.AddCoins(coinsToAdd);

            if (GameManager.instance.DoubleScore)
            {
                GameManager.instance.AddScore(scoreToAdd * 2);
            }
            else
            {
                GameManager.instance.AddScore(scoreToAdd);
            }

            switch (gameObject.tag)
            {
                case "Asteroid":
                    gameObject.GetComponent<AsteroidScript>().SpawnChildAsteroids();
                    break;

                default:
                    gameObject.GetComponent<ItemDrop>().DropItem();
                    Destroy(gameObject);
                    break;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.GetComponent<Player>().isInvincible)
            {
                collision.GetComponent<Player>().TakeDamage(damageDealt);
                Destroy(gameObject);
            }
        }
    }

    private void DealDamage(int damageDealt)
    {
        enemyHealth -= damageDealt;
    }

   
}