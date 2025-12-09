using UnityEngine;

public class EnemySpawnerA : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public int maxEnemies = 5;
    private int spawnedEnemies = 0;

    [Header("Activation")]
    public float activationDistance = 25f;
    private bool isActivated = false;

    [Header("Spawn Area (local)")]
    public Vector2 areaSize = new Vector2(5f, 6f);
    public float spawnXOffset = 2f;

    private float timer;

    private void Update()
    {
        if (spawnedEnemies >= maxEnemies)
        {
            Destroy(gameObject);
            return;
        }

        if (!isActivated)
        {
            float distance = transform.position.x - Camera.main.transform.position.x;
            if (distance < activationDistance) isActivated = true;
            else return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(
            new Vector3(1f, 0.5f, Camera.main.nearClipPlane)
        );

        float spawnX = rightEdge.x + spawnXOffset;

        float spawnY = transform.position.y + Random.Range(-areaSize.y / 2, areaSize.y / 2);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, transform.position.z);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        spawnedEnemies++;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawCube(transform.position, areaSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, areaSize);

        if (Camera.main == null) return;

        Vector3 activationPos = new Vector3(
            transform.position.x - activationDistance,
            transform.position.y,
            transform.position.z
        );

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            activationPos + Vector3.up * 10f,
            activationPos + Vector3.down * 10f
        );

        Gizmos.DrawSphere(activationPos, 0.2f);
    }
}