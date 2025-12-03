using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
        rb.velocity = Vector2.right * speed;
        Invoke(nameof(DestroyBullet), lifetime);
    }
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        EnemyJetA enemy = other.GetComponent<EnemyJetA>();
        if (enemy != null)
        {
            enemy.Explode();  // This triggers the explosion effect
        }
        else
        {
            Destroy(other.gameObject);  // fallback
        }
    }

    Destroy(gameObject);
}
}
