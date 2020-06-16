﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int scoreToAdd = 10;
    [SerializeField] private int coinsToAdd = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            enemyHealth--;
        }

        if (enemyHealth <= 0)
        {
            print("Generic Destory");
            GameManager.Instance.AddCoins(coinsToAdd);
            GameManager.Instance.AddScore(scoreToAdd);
            Destroy(gameObject);
        }
    }
}