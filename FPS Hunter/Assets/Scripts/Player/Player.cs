using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _currentHealth;
    public int playerId;
    
    public float maxHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
