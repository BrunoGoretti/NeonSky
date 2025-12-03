using System.Collections;
using UnityEngine;

public class BottomPipeSpawner : MonoBehaviour
{
    public GameObject prefab;
    
    public float minSpawnRate = 0.5f;  
    public float maxSpawnRate = 2f;   
    
    public float minYOffset = -2f;    
    public float maxYOffset = 0f;     

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn();

            float waitTime = Random.Range(minSpawnRate, maxSpawnRate);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void Spawn()
    {
        float bottomY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float randomY = bottomY + Random.Range(minYOffset, maxYOffset);

        float rightX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1f;

        Vector3 spawnPosition = new Vector3(rightX, randomY, 0);

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}