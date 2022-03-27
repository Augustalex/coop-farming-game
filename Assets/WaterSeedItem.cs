using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UseOnWater))]
public class WaterSeedItem : MonoBehaviour
{
    public Goods.GoodsType goodsType = Goods.GoodsType.RodPlant;
    public GameObject seedTemplate;
    private UseOnWater _useOnWater;
    public Material materialLabel;
    
    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;

        _useOnWater = GetComponent<UseOnWater>();
    }

    private void OnUse(Vector3 highlightPosition)
    {
        var river = _useOnWater.HoveringWater(highlightPosition);
        if (river)
        {
            var soilBlock = river.GetComponent<RiverSoil>();
            if (soilBlock.IsFree())
            {
                OnUseOnSoil(soilBlock);
            }
        }
    }

    public void OnUseOnSoil(RiverSoil soilBlock)
    {
        if (soilBlock.IsFree())
        {
            SeedSoil(soilBlock);
        }
    }

    private void SeedSoil(RiverSoil soil)
    {
        soil.SeedWithTemplate(seedTemplate);

        Destroy(gameObject);
    }

    public bool CompareToSeedItem(WaterSeedItem item)
    {
        return seedTemplate == item.seedTemplate;
    }

    public Goods.GoodsType GetGoods()
    {
        return goodsType;
    }
}