using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private SingletonManager _singletonManager;
    
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    private int _amountPlayersJoined;
    private float timer;
    public bool playerJoined;
    private void Start()
    {
        _singletonManager = SingletonManager.Instance;
        _amountPlayersJoined = 0;
        _singletonManager.EventManager.OnPlayerJoined += OnPlayerJoined;
        
    }

    private void OnPlayerJoined()
    {
        StartCoroutine(nameof(SpawnEnemies));
        playerJoined = true;
    }

    public void OnPlayerJoin()
    {
        Player[] players = FindAllPlayers();

        foreach (var player in players)
        {
            player.playerId = _amountPlayersJoined;
            
            if (!_singletonManager.players.ContainsKey(_amountPlayersJoined))
            {
                _singletonManager.players.Add(_amountPlayersJoined, player.gameObject);
                _amountPlayersJoined += 1;
            }
        }

        _singletonManager.GameManager.waveState = WaveState.WaveStart;
        _singletonManager.GameManager.gameState = GameState.GameRunning;
        _singletonManager.EventManager.PlayerJoined();
       
    }

    private void Update()
    {
        if (playerJoined)
        {
            timer = _singletonManager.GameManager.RunTimer();
            if (timer > 0)
            {
                _singletonManager.UIManager.message.text = $"the next wave will spawn in "+ string.Format("{0:00}", timer);
            }
            else
            {
                _singletonManager.UIManager.message.text = "";
            }
        }
       
        
    }

    IEnumerator SpawnEnemies()
    {
        while (_singletonManager.GameManager.gameState == GameState.GameRunning)
        {
            yield return new WaitForSeconds(15);
            _singletonManager.GameManager.waveState = WaveState.WaveStart;
            
            if (_singletonManager.GameManager.waveState == WaveState.WaveStart)
            {
                _singletonManager.UIManager.remainingEnemiesDisplay.SetActive(true);
                SpawnWave(_singletonManager.GameManager.currentWave);
        
                yield return new WaitUntil(() => _singletonManager.GameManager.waveState == WaveState.WaveCompleted);
                _singletonManager.GameManager.timer = 15f;
            }
        }
    }

    void SpawnWave(int waveNumber)
    {
        int amountEnemies = 0;
        switch (waveNumber)
        {
            case 0:
                //amountEnemies = _amountPlayersJoined * 4 + 1;
                amountEnemies = 1;
                break;
            case 1:
                amountEnemies = _amountPlayersJoined * 6 + 3;
                break;
            case 2:
                amountEnemies = _amountPlayersJoined * 8 + 5;
                break;
            case 3:
                amountEnemies = _amountPlayersJoined * 10 + 7;
                break;
            case 4:
                amountEnemies = _amountPlayersJoined * 12 + 9;
                break;
            case 5:
                amountEnemies = _amountPlayersJoined * 14 + 11;
                break;
        }

        for (int i = 1; i <= amountEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)]);
            enemy.GetComponent<EnemyAI>().target = _singletonManager.players[Random.Range(0, _singletonManager.players.Count)];
        }
        
    }

    public void KillAllEnemies()
    {
        Enemy[] enemies =  FindAllEnemies();
        foreach (var enemy in enemies)
        {
            enemy.TakeDamage(150);
        }
        _singletonManager.GameManager.waveState = WaveState.WaveCompleted;
    }

    public Enemy[] FindAllEnemies()
    {
       return FindObjectsOfType<Enemy>();
    }

    public int RemainingEnemies()
    {
        return FindAllEnemies().Length;
    }

    public Player[] FindAllPlayers()
    {
        return FindObjectsOfType<Player>();
    }
}

