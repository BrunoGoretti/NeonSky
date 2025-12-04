using UnityEngine;

public class EnemySpawnerA : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;

    public float spawnYMin = -3f;
    public float spawnYMax = 3f;

    public int maxEnemies = 5;
    private int spawnedEnemies = 0;

    public float activationDistance = 25f;      private bool isActivated = false;

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

            if (distance < activationDistance)
            {
                isActivated = true;
            }
            else
            {
                return;  
            }
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
    Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, Camera.main.nearClipPlane));
    
    float spawnX = rightEdge.x + 1.5f;

    Vector3 spawnPos = new Vector3(spawnX, Random.Range(spawnYMin, spawnYMax), transform.position.z);

    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

    spawnedEnemies++;
}
}