using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goods"))
        {
            var goods = other.GetComponent<Goods>();
            var deliveryManager = DeliveryManager.Instance;
            if (deliveryManager.Fulfilling(goods.goodsType))
            {
                goods.Consume();
                deliveryManager.OnDelivery(goods.goodsType);
            }
        }
        else if (other.CompareTag("Item"))
        {
            var waterSeedItem = other.GetComponent<WaterSeedItem>();
            if (waterSeedItem)
            {
                var goodsType = waterSeedItem.GetGoods();
                var deliveryManager = DeliveryManager.Instance;
                if (deliveryManager.Fulfilling(goodsType))
                {
                    deliveryManager.OnDelivery(goodsType);
                    
                    waterSeedItem.GetComponent<PlayerItem>().Steal();
                    Destroy(waterSeedItem.gameObject);
                }
            }
        }
    }
}