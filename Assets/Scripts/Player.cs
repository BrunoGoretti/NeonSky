using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public GameObject explosionPrefab;
    public Sprite[] sprites;
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
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.08f, 0.08f);
    }

    private void OnEnable()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
        verticalVelocity = 0f;

        rb.velocity = Vector2.zero;
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
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxVerticalSpeed, maxVerticalSpeed);

        Vector2 velocity = new Vector2(horizontalInput * horizontalSpeed, verticalVelocity);

        rb.velocity = velocity;
    }

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Obstacle"))
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.SetParent(other.transform);
        gameObject.SetActive(false);
    }
    else if (other.CompareTag("Scoring"))
    {
        FindObjectOfType<GameManager>()?.IncreaseScore();
    }
}
}