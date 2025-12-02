using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFlash : MonoBehaviour
{
    public Sprite[] frames;       
    public float frameDuration = 0.05f; 

    private SpriteRenderer sr;
    private int currentFrame = 0;
    private float timer = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning("FlashAnimation: No frames assigned!");
            Destroy(gameObject);
            return;
        }
        sr.sprite = frames[0];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                Destroy(gameObject); 
                return;
            }

            sr.sprite = frames[currentFrame];
        }
    }
}
