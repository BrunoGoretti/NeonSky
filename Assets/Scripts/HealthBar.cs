using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private bool smoothTransition = true; 
    [SerializeField] private float smoothSpeed = 8f;

    private Image healthFillImage;
    private int maxHealth;

    private void Awake()
    {
        healthFillImage = GetComponent<Image>();
        healthFillImage.type = Image.Type.Filled;
        healthFillImage.fillMethod = Image.FillMethod.Horizontal;
        healthFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            maxHealth = playerHealth.MaxHealth;
            UpdateHealthBar();
        }
    }

    private void Update()
    {
        if (playerHealth != null)
            UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float targetFill = (float)playerHealth.CurrentHealth / maxHealth;

        if (smoothTransition)
        {
            healthFillImage.fillAmount = Mathf.Lerp(
                healthFillImage.fillAmount,
                targetFill,
                smoothSpeed * Time.unscaledDeltaTime
            );
        }
        else
        {
            healthFillImage.fillAmount = targetFill;
        }
        if (targetFill > 0.6f)
            healthFillImage.color = Color.green;
        else if (targetFill > 0.3f)
            healthFillImage.color = Color.Lerp(Color.yellow, new Color(1f, 0.5f, 0f), (0.6f - targetFill) / 0.3f);
        else
            healthFillImage.color = Color.red;
    }
}