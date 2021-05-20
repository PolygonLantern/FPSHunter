using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    private SingletonManager _singletonManager;
    public int maxHealth;

    private void Start()
    {   
        _singletonManager = SingletonManager.Instance;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth > 0 )
        {
            _currentHealth -= damage;
        }
        else
        {
            if (_singletonManager.EnemySpawnManager.FindAllEnemies().Length <= 1)
            {
                _singletonManager.GameManager.CompleteWave();
            }

            _singletonManager.EnemySpawnManager.RemainingEnemies();
            Destroy(gameObject);
        }
    }

    
}
