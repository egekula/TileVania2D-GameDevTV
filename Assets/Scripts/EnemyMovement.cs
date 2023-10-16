using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    private Rigidbody2D _enemyRb;
    private void Awake()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        _enemyRb.velocity = new Vector2(enemySpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemySpeed = -enemySpeed;
        FlipEnemySprite();
    }

    private void FlipEnemySprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_enemyRb.velocity.x)), 1f);
    }
    
}
