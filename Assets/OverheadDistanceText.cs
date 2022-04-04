using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInfoTriggerTarget))]
public class OverheadDistanceText : MonoBehaviour
{
    private string _originalText;
    private TMP_Text _text;
    private float _hideUntil;
    private CinemachineVirtualCamera _camera;
    private PlayerItem _playerItem;
    private bool _visible = true;

    private const float HideDelay = 2f;

    void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _originalText = _text.text;

        GetComponent<PlayerInfoTriggerTarget>().Triggered += TurnOff;

        _camera = GameManager.Instance.mainVirtualCamera;
    }

    private void Hide()
    {
        _visible = false;
    }

    private void Show()
    {
        _visible = true;
    }

    private void TurnOff()
    {
        _hideUntil = Time.time + HideDelay;
    }

    void Update()
    {
        var current = transform.rotation;
        transform.LookAt(_camera.transform);
        var newRotation = transform.rotation;
        transform.rotation = new Quaternion(
            current.x,
            newRotation.y,
            current.z,
            newRotation.w
        );

        if (!_visible || Time.time < _hideUntil || Time.time < 30)
        {
            _text.text = "";
        }
        else
        {
            _text.text = _originalText;
        }
    }
}