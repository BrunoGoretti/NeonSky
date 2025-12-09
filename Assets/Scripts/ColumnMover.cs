using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnMover : MonoBehaviour
{
    [HideInInspector] public float parallaxSpeed;
    [HideInInspector] public float riseSpeed;
    [HideInInspector] public float activationDistance;
    [HideInInspector] public Transform player;
    [HideInInspector] public float maxHeight;

    private bool isRising = false;

    private void Update()
    {
        // Move left continuously
        transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

        // Destroy when off-screen left
        if (Camera.main.WorldToViewportPoint(transform.position).x < -1f)
        {
            Destroy(gameObject);
            return;
        }

        if (!isRising && player != null)
        {
            float distanceX = transform.position.x - player.position.x;
            if (distanceX < activationDistance)
                isRising = true;
        }

        if (isRising)
        {
            if (transform.position.y < maxHeight)
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            else
                isRising = false;
        }
    }
}