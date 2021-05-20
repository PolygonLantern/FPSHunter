using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-5)]
public class SingletonManager : MonoBehaviour
{
    public static SingletonManager Instance;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    
    public GameManager GameManager;
    public EnemySpawnManager EnemySpawnManager;
    public EventManager EventManager;
    public UIManager UIManager;
    public AnimationManager AnimationManager;
    
    //Check to make sure there is no other singleton manager running
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);    
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
