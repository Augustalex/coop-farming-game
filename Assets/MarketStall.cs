using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MarketStall : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private bool _on;
    private MarketItemPicker _itemPicker;

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
    }

    void Start()
    {
        GetComponent<PlayerInteractable>().OnInteract += Interact;
    }

    private void Interact()
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
                playerController.SetUIController(_itemPicker);
            }

            GameManager.Instance.FocusCamera(_camera);

            _itemPicker.ActivateUI();
        }
    }
}