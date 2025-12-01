using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Sprite[] explosionSprites; 
    private SpriteRenderer sr;
    private int index = 0;
    public float frameRate = 0.08f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        index = 0;
        sr.sprite = explosionSprites[index];
        InvokeRepeating(nameof(Animate), frameRate, frameRate);
    }

private void Animate()
{
    index++;
    if (index >= explosionSprites.Length)
    {
        CancelInvoke(nameof(Animate));
        Destroy(gameObject);

        FindObjectOfType<GameManager>().GameOver();  
        return;
    }

    sr.sprite = explosionSprites[index];
}
}
