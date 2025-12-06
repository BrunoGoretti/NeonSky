using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector2.left * speed; 
        Invoke(nameof(DestroyBullet), lifetime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}