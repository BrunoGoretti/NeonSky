using UnityEngine;

public class TruckSpawner : MonoBehaviour
{
    [Header("Truck Prefabs")]
    public GameObject[] truckPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 8f;
    public int maxTrucks = 999;
    public bool spawnFromBottom = true;

    [Header("Spawn Area (horizontal spread)")]
    public Vector2 spawnAreaSize = new Vector2(20f, 2f);

    [Header("Activation & Movement")]
    public float activationDistance = 100f; 
    public float moveSpeed = 3f;
    private bool isActivated = false;

    private float timer;
    private int spawnedCount = 0;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (Camera.main.WorldToViewportPoint(transform.position).x < -1f)
        {
            Destroy(gameObject);
            return;
        }

        if (spawnedCount >= maxTrucks) return;

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
            SpawnTruck();
            timer = 0f;
        }
    }

    void SpawnTruck()
    {
        if (truckPrefabs.Length == 0) return;

        GameObject prefab = truckPrefabs[Random.Range(0, truckPrefabs.Length)];

        float spawnY = spawnFromBottom
            ? Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.1f, Camera.main.nearClipPlane)).y
            : Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.1f, Camera.main.nearClipPlane)).y;

        Vector3 spawnPos = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnY,
            transform.position.z
        );

        GameObject truck = Instantiate(prefab, spawnPos, Quaternion.identity);
        TruckMover mover = truck.GetComponent<TruckMover>();
        if (mover != null)
        {
            mover.direction = spawnFromBottom ? Vector2.up : Vector2.down;
            mover.parallaxSpeed = moveSpeed;
        }

        spawnedCount++;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan * 0.3f;
        Gizmos.DrawCube(transform.position, new Vector3(spawnAreaSize.x, 20f, 1f));

        Vector3 actLine = new Vector3(transform.position.x - activationDistance, transform.position.y, transform.position.z);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(actLine + Vector3.up * 25f, actLine + Vector3.down * 25f);
        Gizmos.DrawWireSphere(actLine, 1.5f);
    }
}