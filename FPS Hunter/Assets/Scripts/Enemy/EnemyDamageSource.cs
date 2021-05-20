using System;
using System.Collections;
using UnityEngine;

public class EnemyDamageSource : MonoBehaviour
{
    private EnemyAI _enemy;
    private bool _hasAttacked;
    private SingletonManager _singletonManager;
    private void Awake()
    {
        _singletonManager = SingletonManager.Instance;
        _enemy = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null && _enemy.alreadyAttacked && !_hasAttacked)
        {   
            player.TakeDamage(2.5f);
            _hasAttacked = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(nameof(Attack));
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(_singletonManager.AnimationManager.enemyAnimations[2].length);
        _hasAttacked = false;
    }

}
