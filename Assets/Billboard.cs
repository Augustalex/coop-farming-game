using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private bool _on;
    private BillboardJobPicker _jobPicker;

    private void Awake()
    {
        _camera = GetComponentInChildren<CinemachineVirtualCamera>();
        _jobPicker = GetComponent<BillboardJobPicker>();

        _jobPicker.Submitted += EndInteraction;
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

        _jobPicker.DeactivateUI();
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
                playerController.SetUIController(_jobPicker);
            }

            GameManager.Instance.FocusCamera(_camera);

            _jobPicker.ActivateUI();
        }
    }
}