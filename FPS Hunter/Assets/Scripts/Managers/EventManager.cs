using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
   public Action OnPlayerJoined;

   public void PlayerJoined()
   {
      OnPlayerJoined?.Invoke();
   }
}
