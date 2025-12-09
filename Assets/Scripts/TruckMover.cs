using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMover : MonoBehaviour
{
    [Header("Truck Movement (Vertical)")]
    public Vector2 direction = Vector2.up;  
    public float speed = 4f;
    
    [Header("Parallax Movement (Horizontal)")]
    public float parallaxSpeed = 3f;
    
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float animationSpeed = 0.1f;

    private int currentSpriteIndex = 0;
    private float timer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (sprites.Length > 0)
        {
            currentSpriteIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);

        if (sprites.Length > 1)
        {
            timer += Time.deltaTime;
            if (timer >= animationSpeed)
            {
                timer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
                spriteRenderer.sprite = sprites[currentSpriteIndex];
            }
        }

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.y < -0.3f || viewportPos.y > 1.3f)
        {
            Destroy(gameObject);
        }
    }
}