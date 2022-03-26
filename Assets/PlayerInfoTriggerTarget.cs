using System;
using UnityEngine;

public class PlayerInfoTriggerTarget : MonoBehaviour
{
    public event Action Triggered;
    
    public void Trigger()
    {
        Triggered?.Invoke();
    }
}
