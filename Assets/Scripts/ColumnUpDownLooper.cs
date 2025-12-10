using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnUpDownLooper : MonoBehaviour
{
    [HideInInspector] public float parallaxSpeed;
    [HideInInspector] public float verticalSpeed;
    [HideInInspector] public float maxHeight;
    [HideInInspector] public float minHeight;

    private bool movingUp = true;

    void Update()
    {
        transform.position += Vector3.left * parallaxSpeed * Time.deltaTime;

        Vector3 pos = transform.position;

        if (movingUp)
        {
            pos.y += verticalSpeed * Time.deltaTime;
            if (pos.y >= maxHeight)
            {
                pos.y = maxHeight;
                movingUp = false;
            }
        }
        else
        {
            pos.y -= verticalSpeed * Time.deltaTime;
            if (pos.y <= minHeight)
            {
                pos.y = minHeight;
                movingUp = true;
            }
        }

        transform.position = pos;
    }
}
