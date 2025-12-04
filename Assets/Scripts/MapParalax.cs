using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParalax : MonoBehaviour
{
public float parallaxSpeed = 1f;
    public Vector3 direction = Vector3.left;

    private void Update()
    {
        transform.position += direction * parallaxSpeed * Time.deltaTime;
    }
}
