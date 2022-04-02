using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlayerFalling : MonoBehaviour
{
    private PlayerController _playerController;

    void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        Sounds.Instance.PlayPlayerJoinedSound(Camera.main.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerController.IsFalling())
        {
            _playerController.StopFalling();
            Sounds.Instance.PlayTouchdownSound(transform.parent.position);
            Destroy(gameObject);
        }
    }
}
