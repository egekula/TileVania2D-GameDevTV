using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rbOfBullet;
    [SerializeField] private float bulletSpeed;
    public PlayerMovement playerMovement;
    private float xSpeed;
    [SerializeField] private float aim = 10f;
    
    private void Awake()
    {
        _rbOfBullet = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    private void Update()
    {
        BulletMove();
    }

    private void BulletMove()
    {
        _rbOfBullet.velocity = new Vector2(xSpeed, 0f);
        float positionOfBullet = transform.position.x;
        if (Mathf.Abs(positionOfBullet - playerMovement.gunTransform.position.x) >= aim)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
