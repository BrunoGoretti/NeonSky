using System.Collections.Generic;
using UnityEngine;

public class EnemyJetA : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Sprite[] sprites;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Smart Avoidance")]
    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private float avoidanceForce = 10f;
    [SerializeField] private float maxVerticalSpeed = 7f;
    [SerializeField] private float safeBuffer = 2.5f;
    [SerializeField] private float maxYOffset = 6f;
    [SerializeField] private float minYOffset = 5f;

    [Header("Player Tracking")]
    [SerializeField] private float playerTrackingStrength = 2.2f;
    [SerializeField] private float trackingActivationDistance = 16f;

    private SpriteRenderer spriteRenderer;
    private int spriteIndex;
    private float nextFireTime;
    private Vector3 startPosition;
    private float desiredYVelocity;
    private float currentYVelocity;
    private Transform player;

    private readonly Vector2[] scanDirections = {
        Vector2.left,
        Vector2.left + Vector2.down * 0.7f,
        Vector2.left + Vector2.up * 0.7f,
        Vector2.down,
        Vector2.up
    };

    private void Awake() => startPosition = transform.position;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(AnimateSprite), 0.08f, 0.08f);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        Move();
        if (Time.time >= nextFireTime) Shoot();

        if (Camera.main.WorldToViewportPoint(transform.position).x < -0.2f)
            Destroy(gameObject);
    }

    private void Move()
    {
        transform.position += Vector3.left * (baseMoveSpeed * Time.deltaTime);

        desiredYVelocity = CalculateAvoidance().y;
        currentYVelocity = Mathf.Lerp(currentYVelocity, desiredYVelocity, Time.deltaTime * 12f);
        currentYVelocity = Mathf.Clamp(currentYVelocity, -maxVerticalSpeed, maxVerticalSpeed);
        transform.position += Vector3.up * (currentYVelocity * Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, startPosition.y - maxYOffset, startPosition.y + minYOffset);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    private Vector2 CalculateAvoidance()
    {
        Vector2 pos = transform.position;
        Vector2 avoidanceForceTotal = Vector2.zero;
        float closestThreat = detectionDistance;
        bool hasCriticalThreat = false;

        foreach (Vector2 dir in scanDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(pos, dir.normalized, detectionDistance, obstacleLayer);

            if (hit.collider != null)
            {
                float dist = hit.distance;
                if (dist < closestThreat)
                    closestThreat = dist;

                if (dist < detectionDistance * 0.6f)
                    hasCriticalThreat = true;

                float strength = Mathf.Clamp01(1f - (dist / detectionDistance));
                strength *= strength;

                avoidanceForceTotal += CalculateAvoidanceDirection(dir, pos, strength);
            }
        }

        if (hasCriticalThreat || closestThreat < detectionDistance * 0.7f)
        {
            if (avoidanceForceTotal != Vector2.zero)
                return avoidanceForceTotal.normalized * avoidanceForce * 1.5f;

            return Vector2.zero;
        }

        Vector2 trackingForce = Vector2.zero;

        if (player != null)
        {
            float xDistanceToPlayer = player.position.x - pos.x;

            if (xDistanceToPlayer > -8f && xDistanceToPlayer < trackingActivationDistance)
            {
                float yDiff = player.position.y - pos.y;
                float trackingStrength = playerTrackingStrength;

                if (Mathf.Abs(yDiff) < 1.5f)
                    trackingStrength *= 0.6f;

                Vector2 desiredDirection = Vector2.up * Mathf.Sign(yDiff);
                bool pathClear = !Physics2D.Raycast(
                    pos,
                    desiredDirection,
                    4f,
                    obstacleLayer
                );

                if (pathClear || Mathf.Abs(yDiff) < 1f)
                {
                    trackingForce = desiredDirection * trackingStrength * 0.35f;
                }
            }
        }

        Vector2 finalForce = avoidanceForceTotal + trackingForce;

        if (finalForce.sqrMagnitude < 0.5f && (player == null || Mathf.Abs(player.position.x - pos.x) > trackingActivationDistance + 10f))
        {
            float returnY = (startPosition.y - pos.y) * 0.4f;
            finalForce += Vector2.up * returnY;
        }

        return finalForce.normalized * avoidanceForce;
    }

    private Vector2 CalculateAvoidanceDirection(Vector2 dir, Vector2 position, float strength)
    {
        if (dir.y < -0.3f) return Vector2.up * strength;
        if (dir.y > 0.3f) return Vector2.down * strength;

        bool spaceAbove = !Physics2D.Raycast(position + Vector2.up * safeBuffer, Vector2.left, detectionDistance, obstacleLayer);
        bool spaceBelow = !Physics2D.Raycast(position + Vector2.down * safeBuffer, Vector2.left, detectionDistance, obstacleLayer);

        if (spaceAbove && !spaceBelow)
            return Vector2.up * strength * 1.5f;
        else if (!spaceAbove && spaceBelow)
            return Vector2.down * strength * 1.5f;
        else if (spaceAbove && spaceBelow)
            return Vector2.up * strength * 1.2f;

        return Vector2.up * strength;
    }

    private void Shoot()
    {
        if (!enemyBulletPrefab || !firePoint) return;

        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb) rb.velocity = Vector2.left * 20f;

        if (flashPrefab)
            Instantiate(flashPrefab, firePoint.position, firePoint.rotation, firePoint);

        nextFireTime = Time.time + fireRate;
    }

    private void AnimateSprite()
    {
        if (sprites.Length == 0) return;
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Obstacle")) return;
        
        Explode();
    }

    public void Explode()
    {
        if (explosionPrefab)
        {
            GameObject exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Explosion explosion = exp.GetComponent<Explosion>();
            if (explosion) explosion.isPlayerExplosion = false;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 pos = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, Vector2.left * detectionDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(pos, (Vector2.left + Vector2.down * 0.7f).normalized * detectionDistance);
        Gizmos.DrawRay(pos, (Vector2.left + Vector2.up * 0.7f).normalized * detectionDistance);
    }
}