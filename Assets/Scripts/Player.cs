using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public GameObject explosionPrefab;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public Sprite[] sprites;
    public GameObject flashPrefab;

    public float bulletSpeed = 20f;
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;
    private int spriteIndex;

    public float horizontalSpeed = 10f;
    private float verticalVelocity = 0f;
    private float maxVerticalSpeed = 14f;
    private float verticalAcceleration = 14f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 5f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.08f, 0.08f);
    }

    private void OnEnable()
    {
        Vector3 pos = transform.position;
        pos.y = 2f;
        transform.position = pos;
        verticalVelocity = 0f;

        rb.velocity = Vector2.zero;
    }


    private void Update()
    {
        HandleShooting();

    }
    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = transform.right * bulletSpeed;
        }

        if (flashPrefab != null)
        {
            Instantiate(flashPrefab, firePoint.position, firePoint.rotation, firePoint);
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;

        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.W))
            verticalInput = 2f;
        else if (Input.GetKey(KeyCode.S))
            verticalInput = -1f;

        verticalVelocity += verticalInput * verticalAcceleration * Time.fixedDeltaTime;
        verticalVelocity *= 0.95f;

        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxVerticalSpeed, maxVerticalSpeed);

        Vector2 velocity = new Vector2(horizontalInput * horizontalSpeed, verticalVelocity);

        rb.velocity = velocity;
    }

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }
    private void LateUpdate()
    {
        KeepPlayerInsideScreen();
    }
    private void KeepPlayerInsideScreen()
    {
        Vector3 pos = transform.position;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.00f, 1f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.00f, 1f);

        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        ExplodePlayer();

        EnemyJetA enemy = other.GetComponent<EnemyJetA>();
        if (enemy != null)
        {
            enemy.Explode();
        }
    }
    else if (other.CompareTag("Obstacle"))
    {
        ExplodePlayer();

        Pipes pipeScript = other.GetComponent<Pipes>();
        if (pipeScript != null && explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Explosion explosionScript = explosion.GetComponent<Explosion>();
            if (explosionScript != null)
            {
                explosionScript.isPlayerExplosion = true;
                explosionScript.moveSpeed = pipeScript.speed;
            }
        }
    }
}

public void ExplodePlayer()
{
    if (explosionPrefab != null)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity)
            .GetComponent<Explosion>().isPlayerExplosion = true;
    }
    gameObject.SetActive(false);
}
}