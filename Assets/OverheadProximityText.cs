using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInfoTriggerTarget))]
public class OverheadProximityText : MonoBehaviour
{
    private string _originalText;
    private TMP_Text _text;
    private float _endOfShow;
    private CinemachineVirtualCamera _camera;
    private PlayerItem _playerItem;
    private bool _visible = true;

    private const float HideDelay = 2f;

    void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _originalText = _text.text;

        GetComponent<PlayerInfoTriggerTarget>().Triggered += TurnOn;
        _playerItem = GetComponentInParent<PlayerItem>();
        if (_playerItem)
        {
            _playerItem.Grabbed += Hide;
            _playerItem.Dropped += Show;
        }

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

    private void TurnOn()
    {
        _endOfShow = Time.time + HideDelay;
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

        if (!_visible || Time.time > _endOfShow)
        {
            _text.text = "";
        }
        else
        {
            _text.text = _originalText;
        }
    }
}