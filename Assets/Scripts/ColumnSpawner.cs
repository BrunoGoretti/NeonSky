using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject columnPrefab;

    [Header("Movement Settings")]
    public float parallaxSpeed = 3f;
    public float riseSpeed = 4f;

    [Header("Height Settings")]
    public float spawnYOffset = 5f;   // How far below spawner to spawn (underground)
    public float maxHeight = 10f;     // Max height column should rise to

    [Header("Activation")]
    public float activationDistance = 50f;

    [Header("Player Reference")]
    public Transform player;

    // Store column size for Gizmos visualization
    private float columnWidth = 2f;
    private float columnHeight = 10f;

    private bool hasSpawned = false;

    private void Start()
    {
        // Try to get size from prefab's SpriteRenderer
        if (columnPrefab != null)
        {
            var spriteRenderer = columnPrefab.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                columnWidth = spriteRenderer.bounds.size.x;
                columnHeight = spriteRenderer.bounds.size.y;
            }
        }
    }

    private void Update()
    {
        if (!hasSpawned)
        {
            SpawnColumn();
            hasSpawned = true;
        }
    }

    private void SpawnColumn()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - spawnYOffset, transform.position.z);

        GameObject col = Instantiate(columnPrefab, spawnPos, Quaternion.identity);

        ColumnMover mover = col.GetComponent<ColumnMover>();
        if (mover != null)
        {
            mover.parallaxSpeed = parallaxSpeed;
            mover.riseSpeed = riseSpeed;
            mover.activationDistance = activationDistance;
            mover.player = player;

            mover.maxHeight = maxHeight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Activation Distance Visualization
        Gizmos.color = Color.yellow;
        Vector3 activationPos = transform.position + Vector3.left * activationDistance;
        Gizmos.DrawLine(transform.position + Vector3.up * 2f, activationPos + Vector3.up * 2f);
        Gizmos.DrawWireSphere(activationPos + Vector3.up * 2f, 1f);

        // Start position underground (where column spawns)
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y - spawnYOffset, transform.position.z);
        // Max height position where column stops rising
        Vector3 maxPos = new Vector3(transform.position.x, maxHeight, transform.position.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, maxPos);

        // Cube size (width, height, depth)
        Vector3 cubeSize = new Vector3(columnWidth, columnHeight, 1f);

        // Draw cube at startPos, shifted up by half height so bottom aligns with startPos
        Gizmos.DrawWireCube(startPos + Vector3.up * (columnHeight / 2f), cubeSize);

        // Draw cube at maxPos, shifted up by half height so bottom aligns with maxPos
        Gizmos.DrawWireCube(maxPos + Vector3.up * (columnHeight / 2f), cubeSize);
    }
}