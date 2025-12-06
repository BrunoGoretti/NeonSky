using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private Player player;

    public int CurrentHealth => currentHealth; 
    public int MaxHealth => maxHealth;         

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        if (player != null)
            player.ExplodePlayer();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}