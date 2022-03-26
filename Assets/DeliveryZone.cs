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
            DeliveryManager.Instance.OnDelivery(goods);
        }
    }
}
