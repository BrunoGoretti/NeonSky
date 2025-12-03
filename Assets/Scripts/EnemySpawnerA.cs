using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerA : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;

    public float spawnYMin = -3f;
    public float spawnYMax = 3f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(0f, 1f), 10f));
        spawnPos.y = Random.Range(spawnYMin, spawnYMax); 

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}