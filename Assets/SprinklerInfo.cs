using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SprinklerInfo : MonoBehaviour
{
    private WaterLevelIndicator _waterInfo;

    private float _endOfShowTime;
    private GameObject _waterInfoRoot;
    private SprinklerShooter _sprinkler;
    private const float HideDelay = 2f;

    void Awake()
    {
        _waterInfo = GetComponentInChildren<WaterLevelIndicator>();
        _waterInfoRoot = _waterInfo.gameObject;

        GetComponent<PlayerInfoTriggerTarget>().Triggered += TriggerShow;

        _sprinkler = GetComponentInParent<SprinklerShooter>();
    }

    private void OnWatered(float waterTime)
    {
        var waterLevel = Mathf.FloorToInt((waterTime / _sprinkler.GetMaxWaterTime()) * 4f);

        _waterInfo.OnChange(waterLevel, (int) 4, WaterLevelIndicator.WaterLevelIndicatorState.Wet,
            WaterLevelIndicator.WaterLevelIndicatorSize.Large);
    }

    void Update()
    {
        if (Time.time > _endOfShowTime)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void TriggerShow()
    {
        _endOfShowTime = Time.time + HideDelay;
    }

    private void Show()
    {
        OnWatered(_sprinkler.GetWaterTime());
        _waterInfoRoot.SetActive(true);
    }

    private void Hide()
    {
        _waterInfoRoot.SetActive(false);
    }
}