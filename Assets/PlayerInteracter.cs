using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracter : MonoBehaviour
{
    public GameObject highlight;
    private PlayerGrabber _grabber;
    private PlayerController _playerController;

    void Start()
    { 
        _grabber = GetComponent<PlayerGrabber>();
        _playerController = GetComponent<PlayerController>();
    }
    
    void OnGrab(InputValue value)
    {
        if (value.isPressed)
        {
            if (!_grabber.HasItem())
            {
                var hits = Physics.OverlapSphere(highlight.transform.position, .5f)
                    .Where(hit => hit.GetComponent<PlayerInteractable>()).ToArray();
                if (hits.Length > 0)
                {
                    var hit = hits[0];
                    hit.GetComponent<PlayerInteractable>().Interact(_playerController);
                }
            }
        }
    }
}
