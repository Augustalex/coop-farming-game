using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    public enum GoodsType
    {
        YellowFlower,
        RedBerry,
        WaterPlant,
        RodPlant
    }

    public GoodsType goodsType = GoodsType.YellowFlower;

    public int count = 1;

    public void Consume()
    {
        Destroy(gameObject, .1f);
    }
}
