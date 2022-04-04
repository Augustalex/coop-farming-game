using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(UseOnGrass))]
[RequireComponent(typeof(UseOnSoil))]
[RequireComponent(typeof(CountData))]
public class WaterCanItem : MonoBehaviour
{
    private PlayerItem _playerItem;

    public GameObject hasWaterStyle;
    public GameObject noWaterStyle;
    private CountData _countData;
    private UseOnSoil _useOnSoil;
    private UseOnGrass _useOnGrass;
    private float _lastRecharge;
    private SmartGhost _smartGhost;
    private ActionGhost _actionGhost;

    void Start()
    {
        _countData = GetComponent<CountData>();
        _useOnSoil = GetComponent<UseOnSoil>();
        _useOnGrass = GetComponent<UseOnGrass>();

        _smartGhost = GetComponent<SmartGhost>();
        if (_smartGhost)
        {
        }
        _actionGhost = GetComponent<ActionGhost>();

        _playerItem = GetComponent<PlayerItem>();
        _playerItem.UseItem += OnUseItem;

        noWaterStyle.SetActive(false);
    }

    void Update()
    {
        if (_smartGhost.Activated())
        {
            _actionGhost.Deactivate();
        }
        else
        {
            _actionGhost.Activate();
        }
    }

    private void OnUseItem(Vector3 highlightPosition)
    {
        if (_countData.count == 0) return;

        if (_smartGhost.Activated())
        {
            var soil = _useOnSoil.HoverOnSoil(highlightPosition);

            if (soil)
            {
                WaterSoil(soil);
                Sounds.Instance.PlayWaterSound(transform.position);
            }
            else
            {
                var grass = _useOnGrass.HoveringGrass(highlightPosition);
                if (grass)
                {
                    var grassBlock = grass.GetComponent<GrassBlock>();
                    if (grassBlock && grassBlock.HasPlant())
                    {
                        WaterGrass(grassBlock);
                        Sounds.Instance.PlayWaterSound(transform.position);
                    }
                }
            }
        }
        else
        {
            if (_actionGhost.IsValidLocation(highlightPosition))
            {
                var hits = _actionGhost.HitsForPosition(highlightPosition);
                var hit = hits[0];
                var sprinkler = hit.GetComponent<SprinklerShooter>();
                sprinkler.Water(ConsumeOneWater());
                Sounds.Instance.PlayWaterSound(transform.position);
            }
        }
    }

    private void WaterGrass(GrassBlock grassBlock)
    {
        var flower = grassBlock.GetPlant().GetComponent<FlowerGrowth>();
        flower.Water();

        UseWater();
    }

    private void WaterSoil(GameObject soil)
    {
        var soilBlock = soil.GetComponent<SoilBlock>();
        soilBlock.Water();
        UseWater();
    }

    private void UseWater()
    {
        _countData.count -= 1;
        if (_countData.count <= 0)
        {
            SetHasNoWater();
        }
    }

    public float GetWaterPercentage()
    {
        return _countData.count / _countData.max;
    }

    public void OnWaterZone()
    {
        if (Time.time - _lastRecharge < 1f) return;
        if (_countData.count == _countData.max) return;

        _lastRecharge = Time.time;

        _countData.count = _countData.max;
        Sounds.Instance.PlayerWaterRechargeSound(transform.position);
        SetHasWater();
    }

    private void SetHasNoWater()
    {
        noWaterStyle.SetActive(true);
        hasWaterStyle.SetActive(false);
    }

    private void SetHasWater()
    {
        noWaterStyle.SetActive(false);
        hasWaterStyle.SetActive(true);
    }

    public float ConsumeAllExcessWater()
    {
        var percentage = GetWaterPercentage();
        _countData.count = 0;
        SetHasNoWater();

        return percentage;
    }

    public float ConsumeOneWater()
    {
        if (_countData.count == 0) return 0f;
        UseWater();

        return 1f;
    }
}