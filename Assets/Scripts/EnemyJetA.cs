using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJetA : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public GameObject explosionPrefab; 
    public GameObject enemyBulletPrefab;
    public GameObject flashPrefab;
    public Transform firePoint;

    public Sprite[] sprites;
    private int spriteIndex;
    public float moveSpeed = 5f;
 
    public float fireRate = 1.5f; 
    private float nextFireTime = 0f;
    public float verticalAmplitude = 1f;
    public float verticalFrequency = 1f;

    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        InvokeRepeating(nameof(AnimateSprite), 0.08f, 0.08f);
    }

private void Update()
{
    transform.position += Vector3.left * moveSpeed * Time.deltaTime;

    float newY = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

    if (Camera.main.WorldToViewportPoint(transform.position).x < -0.1f)
    {
        Destroy(gameObject);
    }

    if (Time.time >= nextFireTime)
    {
        Shoot();
        nextFireTime = Time.time + fireRate;
    }
}

private void Shoot()
{
    if (enemyBulletPrefab != null && firePoint != null)
    {
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.velocity = Vector2.left * 20f;
        }

        if (flashPrefab != null)
        {
            Instantiate(flashPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
    }
}

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

public void Explode()
{
    if (explosionPrefab != null)
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Explosion explosionScript = explosion.GetComponent<Explosion>();
        if (explosionScript != null)
        {
            explosionScript.isPlayerExplosion = false;
        }
    }
    Destroy(gameObject);
}
}