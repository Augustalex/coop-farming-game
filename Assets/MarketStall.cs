using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MarketStall : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private bool _on;
    private MarketItemPicker _itemPicker;

    public event Action<bool> Toggled;

    private void Awake()
    {
        _camera = GetComponentInChildren<CinemachineVirtualCamera>();
        _itemPicker = GetComponentInChildren<MarketItemPicker>();

        _itemPicker.Submitted += EndInteraction;
    }

    private void EndInteraction()
    {
        _on = false;

        foreach (var playerController in FindObjectsOfType<PlayerController>())
        {
            playerController.ClearUIController();
            playerController.UnFreeze();
        }

        GameManager.Instance.ResetCameraFocus();

        _itemPicker.DeactivateUI();

        Toggled?.Invoke(_on);
    }

    void Start()
    {
        GetComponent<PlayerInteractable>().OnInteract += Interact;
    }

    private void Interact(PlayerController controller)
    {
        if (_on)
        {
            EndInteraction();
        }
        else
        {
            _on = true;

            foreach (var playerController in FindObjectsOfType<PlayerController>())
            {
                playerController.Freeze();
            }

            controller.SetUIController(_itemPicker);

            GameManager.Instance.FocusCamera(_camera);

            _itemPicker.ActivateUI();

            Toggled?.Invoke(_on);
        }
    }
}