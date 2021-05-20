using System;
using UnityEngine;
using UnityEngine.LowLevel;

public enum WaveState
{
    WaveStart,
    WaveCompleted,
}

public enum GameState
{
    GameOver,
    GamePaused,
    GameRunning
}

public class GameManager : MonoBehaviour
{
    private bool _playersReady;
    private bool _anyPlayerReady;
    private SingletonManager _singletonManager;

    public WaveState waveState;
    public GameState gameState;
    public int currentWave;
    public float timer;
    private void Awake()
    {
        gameState = GameState.GameRunning;
        
        timer = 15f;
    }

    private void Start()
    {
        _singletonManager = SingletonManager.Instance;
        currentWave = 0;
        _singletonManager.UIManager.remainingEnemiesDisplay.SetActive(false);

    }

    public float RunTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
        }

        return timer;
    }

    public void CompleteWave()
    {
        waveState = WaveState.WaveCompleted;
        currentWave += 1;
        _singletonManager.UIManager.remainingEnemiesDisplay.SetActive(false);
    }

    private void Update()
    {
        _singletonManager.UIManager.remainingEnemies.text =
            _singletonManager.EnemySpawnManager.RemainingEnemies().ToString();

        if (_singletonManager.EnemySpawnManager.FindAllPlayers().Length <= 0)
        {
            gameState = GameState.GameOver;
        }
        else
        {
            gameState = GameState.GameRunning;
        }
    }
}
