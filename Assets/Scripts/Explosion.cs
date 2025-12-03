using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    public Sprite[] explosionSprites;
    private SpriteRenderer sr;
    private int index = 0;
    public float frameRate = 0.08f;
    public bool isPlayerExplosion = false;
    public float moveSpeed = 5f;

    public GameObject explosionLightPrefab;
    private Light2D explosionLight;  

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
            GameObject instance = Instantiate(explosionLightPrefab, transform.position, Quaternion.identity, transform);
            explosionLight = instance.GetComponent<Light2D>();

            if (explosionLight != null)
                StartCoroutine(FadeLight());
        }
    }

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private IEnumerator FadeLight()
    {
        float duration = explosionSprites.Length * frameRate;
        float time = 0f;
        float startIntensity = explosionLight.intensity;

        while (time < duration)
        {
            time += Time.deltaTime;

            if (explosionLight != null)
            {
                explosionLight.intensity = Mathf.Lerp(startIntensity, 0f, time / duration);
            }

            yield return null;
        }

        if (explosionLight != null)
            Destroy(explosionLight.gameObject);
    }

    private void Animate()
    {
        index++;

        if (index >= explosionSprites.Length)
        {
            CancelInvoke(nameof(Animate));
            Destroy(gameObject);

            if (isPlayerExplosion)
            {
                FindObjectOfType<GameManager>().GameOver();
            }

            return;
        }

        sr.sprite = explosionSprites[index];
    }
}