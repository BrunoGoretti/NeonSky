using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 3f;
    public int damage = 3;

    [Header("Animation")]
    public Sprite[] rocketSprites;
    public float animationSpeed = 0.08f;
    [Header("Explosion")]
    public GameObject explosionPrefab;

    [Header("AOE Damage")]
    public float explosionRadius = 2f;
    public int aoeDamage = 2;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int spriteIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector2.right * speed;

        if (rocketSprites != null && rocketSprites.Length > 0)
            InvokeRepeating(nameof(AnimateSprite), animationSpeed, animationSpeed);

        Invoke(nameof(DestroySelf), lifetime);
    }

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % rocketSprites.Length;
        sr.sprite = rocketSprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyHealth>(out EnemyHealth hp))
                hp.TakeDamage(damage);
        }

        DestroySelf();
    }

    private void DestroySelf()
    {
        CancelInvoke(nameof(AnimateSprite));
        ApplyAOEDamage();

        if (explosionPrefab != null)
        {
            GameObject exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            if (exp.TryGetComponent<Explosion>(out Explosion explosion))
            {
                explosion.isPlayerExplosion = false;
            }
        }

        Destroy(gameObject);
    }

    private void ApplyAOEDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent<EnemyHealth>(out EnemyHealth hp))
                    hp.TakeDamage(aoeDamage);
            }
                        if (hit.CompareTag("Player"))
            {
                if (hit.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
                    hp.TakeDamage(aoeDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, explosionRadius);
}
}
