using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInfoTriggerTarget))]
public class InfoCountDisplay : MonoBehaviour
{
    private TMP_Text _text;
    private CountData _countData;
    private float _endOfShow;

    private const float HideDelay = 2f;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        _countData = GetComponentInParent<CountData>();
        GetComponent<PlayerInfoTriggerTarget>().Triggered += TurnOn;
    }

    private void TurnOn()
    {
        _endOfShow = Time.time + HideDelay;
    }

    void Update()
    {
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