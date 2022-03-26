using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolUser : MonoBehaviour
{
    private PlayerGrabber _grabber;

    void Start()
    { 
        _grabber = GetComponentInChildren<PlayerGrabber>();
    }

    void OnUse(InputValue value)
    {
        if (value.isPressed)
        {
            if (_grabber.HasItem())
            {
                _grabber.UseItem();
            }
        }
    }
}
