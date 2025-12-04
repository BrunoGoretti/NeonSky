using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject flashPrefab;
    
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float maxVerticalSpeed = 14f;
    [SerializeField] private float verticalAcceleration = 14f;
    
    [Header("Shooting")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.25f;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float nextFireTime;
    private float verticalVelocity;
    private int spriteIndex;
    
    private const float SCREEN_BOUNDARY = 0.0f;
    private const float VERTICAL_DAMPENING = 0.95f;
    private const float SPAWN_HEIGHT = 2f;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        rb.gravityScale = 5f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
    }
    
    private void Start() => InvokeRepeating(nameof(AnimateSprite), 0.08f, 0.08f);
    
    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, SPAWN_HEIGHT, transform.position.z);
        verticalVelocity = 0f;
        rb.velocity = Vector2.zero;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
            Shoot();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        KeepPlayerInsideScreen();
    }
    
    private void HandleMovement()
    {
        float horizontalInput = GetHorizontalInput();
        float verticalInput = GetVerticalInput();
        
        verticalVelocity += verticalInput * verticalAcceleration * Time.fixedDeltaTime;
        verticalVelocity *= VERTICAL_DAMPENING;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxVerticalSpeed, maxVerticalSpeed);
        
        rb.velocity = new Vector2(horizontalInput * horizontalSpeed, verticalVelocity);
    }
    
    private float GetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.A)) return -1f;
        if (Input.GetKey(KeyCode.D)) return 1f;
        return 0f;
    }
    
    private float GetVerticalInput()
    {
        if (Input.GetKey(KeyCode.W)) return 2f;
        if (Input.GetKey(KeyCode.S)) return -1f;
        return 0f;
    }
    
    private void Shoot()
    {
        if (!bulletPrefab || !firePoint) return;
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
            bulletRb.velocity = transform.right * bulletSpeed;
        
        if (flashPrefab)
            Instantiate(flashPrefab, firePoint.position, firePoint.rotation, firePoint);
        
        nextFireTime = Time.time + fireRate;
    }
    
    private void AnimateSprite()
    {
        if (sprites.Length == 0) return;
        spriteRenderer.sprite = sprites[spriteIndex = (spriteIndex + 1) % sprites.Length];
    }
    
    private void KeepPlayerInsideScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp(viewportPos.x, SCREEN_BOUNDARY, 1f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, SCREEN_BOUNDARY, 1f);
        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Obstacle")) return;
        
        ExplodePlayer();
        
        if (other.CompareTag("Enemy") && other.TryGetComponent<EnemyJetA>(out EnemyJetA enemy))
            enemy.Explode();
        else if (other.CompareTag("Obstacle"))
            CreateObstacleExplosion(other);
    }
    
    private void CreateObstacleExplosion(Collider2D obstacle)
    {
        if (!explosionPrefab || !obstacle.TryGetComponent<Pipes>(out Pipes pipeScript)) return;
        
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        if (explosion.TryGetComponent<Explosion>(out Explosion explosionScript))
        {
            explosionScript.isPlayerExplosion = true;
            explosionScript.moveSpeed = pipeScript.speed;
        }
    }
    
    public void ExplodePlayer()
    {
        if (!explosionPrefab) return;
        
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        if (explosion.TryGetComponent<Explosion>(out Explosion explosionScript))
            explosionScript.isPlayerExplosion = true;
        
        gameObject.SetActive(false);
    }
}