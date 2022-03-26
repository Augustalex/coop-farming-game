using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class WaterCanItem : MonoBehaviour
{
    private PlayerItem _playerItem;
    private int _water = 3;

    public GameObject hasWaterStyle;
    public GameObject noWaterStyle;
    
    void Start()
    {
        _water = GameManager.Instance.gameSettings.fullWaterCharge;
        
        _playerItem = GetComponent<PlayerItem>();

        _playerItem.UseItem += OnUseItem;
        
        noWaterStyle.SetActive(false);
    }

    private void OnUseItem(Vector3 highlightPosition)
    {
        if (_water > 0)
        {
            var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f).Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
            if (hits.Length > 0)
            {
                var hit = hits[0];
                if (hit.collider.CompareTag("Soil"))
                {
                    WaterSoil(hit.collider.gameObject);
                    Sounds.Instance.PlayWaterSound(transform.position);
                }
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

        _water -= 1;
        if (_water <= 0)
        {
            SetNoWaterStyle(); 
        }
    }

    public void OnWaterZone()
    {
        _water = GameManager.Instance.gameSettings.fullWaterCharge;
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
