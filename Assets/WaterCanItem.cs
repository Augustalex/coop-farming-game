using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(UseOnSoil))]
[RequireComponent(typeof(CountData))]
public class WaterCanItem : MonoBehaviour
{
    private PlayerItem _playerItem;

    public GameObject hasWaterStyle;
    public GameObject noWaterStyle;
    private CountData _countData;
    private UseOnSoil _useOnSoil;
    private float _lastRecharge;

    void Start()
    {
        _countData = GetComponent<CountData>();
        _useOnSoil = GetComponent<UseOnSoil>();

        _playerItem = GetComponent<PlayerItem>();
        _playerItem.UseItem += OnUseItem;

        noWaterStyle.SetActive(false);
    }

    private void OnUseItem(Vector3 highlightPosition)
    {
        if (_countData.count > 0)
        {
            var soil = _useOnSoil.HoveringSoil(highlightPosition);

            if (soil)
            {
                WaterSoil(soil);
                Sounds.Instance.PlayWaterSound(transform.position);
            }
        }
        else
        {
            Sounds.Instance.PlayFailedWaterSound(transform.position);
        }
    }

    private void WaterSoil(GameObject soil)
    {
        var soilBlock = soil.GetComponent<SoilBlock>();
        soilBlock.Water();

        _countData.count -= 1;
        if (_countData.count <= 0)
        {
            SetNoWaterStyle();
        }
    }

    public void OnWaterZone()
    {
        if (Time.time - _lastRecharge < 1f) return;
        if (_countData.count == _countData.max) return;

        _lastRecharge = Time.time;

        _countData.count = _countData.max;
        Sounds.Instance.PlayerWaterRechargeSound(transform.position);
        SetHasWaterStyle();
    }

    private void SetNoWaterStyle()
    {
        noWaterStyle.SetActive(true);
        hasWaterStyle.SetActive(false);
    }

    private void SetHasWaterStyle()
    {
        noWaterStyle.SetActive(false);
        hasWaterStyle.SetActive(true);
    }
}