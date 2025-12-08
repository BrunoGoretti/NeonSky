using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private EnemyJetA enemyJet;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyJet = GetComponent<EnemyJetA>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (enemyJet != null)
            enemyJet.Explode();
    }
}
