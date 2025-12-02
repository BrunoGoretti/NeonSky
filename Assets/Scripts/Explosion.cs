using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    public Sprite[] explosionSprites;
    private SpriteRenderer sr;
    private int index = 0;
    public float frameRate = 0.08f;

    public float moveSpeed = 5f;
    public GameObject explosionLightPrefab;
    private GameObject activeLightInstance;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 10;
    }

    private void OnEnable()
    {
        index = 0;
        sr.sprite = explosionSprites[index];
        InvokeRepeating(nameof(Animate), frameRate, frameRate);

        if (explosionLightPrefab != null)
        {
            activeLightInstance = Instantiate(explosionLightPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void Animate()
    {
        index++;
        if (index >= explosionSprites.Length)
        {
            CancelInvoke(nameof(Animate));

            if (activeLightInstance != null)
            {
                Destroy(activeLightInstance);
            }

            Destroy(gameObject);

            FindObjectOfType<GameManager>().GameOver();
            return;
        }

        sr.sprite = explosionSprites[index];
    }
}