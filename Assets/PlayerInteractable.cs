using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public event Action<PlayerController> OnInteract;
    
    public void Interact(PlayerController playerController)
    {
        OnInteract?.Invoke(playerController);
    }
}
