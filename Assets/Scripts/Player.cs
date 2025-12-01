using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;
    public Sprite[] sprites;
    private int spriteIndex;

    private Vector3 direction;
    public float gravity = -6f;
    public float strength = 10f;
    private float verticalVelocity = 0f;
    private float maxVerticalSpeed = 14f;

    private float verticalAcceleration = 14f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimatedSprite), 0.08f, 0.08f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
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

        verticalVelocity += verticalInput * verticalAcceleration * Time.deltaTime;

        verticalVelocity += gravity * Time.deltaTime;

        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxVerticalSpeed, maxVerticalSpeed);

        Vector3 horizontalMovement = new Vector3(horizontalInput * strength * Time.deltaTime, 0, 0);

        transform.position += horizontalMovement + new Vector3(0, verticalVelocity * Time.deltaTime, 0);
    }

    private void AnimatedSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.CompareTag("Obstacle"))
{
    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    gameObject.SetActive(false);
}
        else if (other.CompareTag("Scoring"))
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }

}
