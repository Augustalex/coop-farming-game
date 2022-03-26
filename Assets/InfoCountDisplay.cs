using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInfoTriggerTarget))]
public class InfoCountDisplay : MonoBehaviour
{
    private TMP_Text _text;
    private CountData _countData;
    private float _endOfShow;
    private CinemachineVirtualCamera _camera;

    private const float HideDelay = 2f;

    void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _countData = GetComponentInParent<CountData>();
        GetComponent<PlayerInfoTriggerTarget>().Triggered += TurnOn;

        _camera = GameManager.Instance.mainVirtualCamera;
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

        if (Time.time > _endOfShow)
        {
            _text.text = "";
        }
        else
        {
            _text.text = _countData.max == 0 ? $"{_countData.count}" : $"{_countData.count}/{_countData.max}";
        }
    }
}