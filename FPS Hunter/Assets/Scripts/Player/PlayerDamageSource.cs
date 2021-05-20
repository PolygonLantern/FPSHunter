using System;
using UnityEngine;

public class PlayerDamageSource : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.collider.GetComponentInParent<Enemy>();
        if (enemy != null && _playerController.attacked)
        {        
            enemy.TakeDamage(5);
        }
    }
}
