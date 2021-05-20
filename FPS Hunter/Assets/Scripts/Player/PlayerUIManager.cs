using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerHealthUI;
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        playerHealthUI.text = $"Health " + Mathf.Ceil(_player.GetCurrentHealth());
    }
}
