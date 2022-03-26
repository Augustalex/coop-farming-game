using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SoilInfo : MonoBehaviour
{
    private HealthIndicator _healthInfo;
    private WaterLevelIndicator _waterInfo;

    private float _endOfShowTime;
    private GameObject _healthInfoRoot;
    private GameObject _waterInfoRoot;
    private SoilBlock _soil;
    private SoilBlock.SoilState _soilState;
    private const float HideDelay = 2f;

    void Awake()
    {
        _healthInfo = GetComponentInChildren<HealthIndicator>();
        _healthInfoRoot = _healthInfo.gameObject;

        _waterInfo = GetComponentInChildren<WaterLevelIndicator>();
        _waterInfoRoot = _waterInfo.gameObject;

        GetComponent<PlayerInfoTriggerTarget>().Triggered += TriggerShow;

        _soil = GetComponentInParent<SoilBlock>();
        _soil.BlockStateChanged += OnBlockStateChanged;
    }

    private void OnBlockStateChanged(SoilBlock.BlockState blockState)
    {
        _soilState = blockState.soilState;
        _healthInfo.OnChange(blockState.levelData.healthLevel, (int) blockState.levelData.maxHealthLevels,
            HealthIndicator.HealthLevelIndicatorSize.Large);
        _waterInfo.OnChange(blockState.levelData.waterLevel, (int) blockState.levelData.maxWaterLevels, blockState.levelData.waterState,
            blockState.levelData.maxWaterLevels > 4
                ? WaterLevelIndicator.WaterLevelIndicatorSize.Small
                : WaterLevelIndicator.WaterLevelIndicatorSize.Large);
    }

    private void Start()
    {
        _healthInfo.OnChange(4, 4,HealthIndicator.HealthLevelIndicatorSize.Large);
        _waterInfo.OnChange(4, 4, WaterLevelIndicator.WaterLevelIndicatorState.Wet,
            WaterLevelIndicator.WaterLevelIndicatorSize.Large);
    }

    void Update()
    {
        if (Time.time > _endOfShowTime || _soilState == SoilBlock.SoilState.Free ||
            _soilState == SoilBlock.SoilState.ReadyToHarvest)
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
        _waterInfoRoot.SetActive(true);
        _healthInfoRoot.SetActive(true);
    }

    private void Hide()
    {
        _waterInfoRoot.SetActive(false);
        _healthInfoRoot.SetActive(false);
    }
}