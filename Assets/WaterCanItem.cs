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

    void Start()
    {
        _countData = GetComponent<CountData>();
        _useOnSoil = GetComponent<UseOnSoil>();
        _useOnGrass = GetComponent<UseOnGrass>();

        _playerItem = GetComponent<PlayerItem>();
        _playerItem.UseItem += OnUseItem;

        noWaterStyle.SetActive(false);
    }

    private void OnUseItem(Vector3 highlightPosition)
    {
        if (_countData.count > 0)
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
                    if (grassBlock.HasPlant())
                    {
                        WaterGrass(grassBlock);
                        Sounds.Instance.PlayWaterSound(transform.position);
                    }
                }
            }
        }
        else
        {
            Sounds.Instance.PlayFailedWaterSound(transform.position);
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
}