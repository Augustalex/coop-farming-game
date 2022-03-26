using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Game;
using UnityEngine;

public class DeliveryManager : MonoSingleton<DeliveryManager>
{
    public DeliveryRequest deliveryRequest;
    public bool hasDelivery = false;

    private void Update()
    {
        if (hasDelivery)
        {
            var timeLeftOfDelivery =
                Mathf.Clamp(deliveryRequest.deadline - Time.time, 0f, 999f);

            if (timeLeftOfDelivery == 0f)
            {
                EndDelivery();
                TruckMover.Instance.DeliveryEnded();
            }
            else if (DeliveryIsSatisfied())
            {
                GameManager.Instance.money += Reward();
                EndDelivery();
                TruckMover.Instance.DeliveryEnded();

                Sounds.Instance.PlayKachingSound(GameManager.Instance.GetCameraPosition());

                GameManager.Instance.jobsDone += 1;
            }
        }
    }

    private int Reward()
    {
        return deliveryRequest.requests.Sum(request => request.need * 20);
    }

    private bool DeliveryIsSatisfied()
    {
        return deliveryRequest.requests.All(request => request.satisfied >= request.need);
    }

    public void OnDelivery(Goods goods)
    {
        deliveryRequest.Provide(goods);
    }

    public void EndDelivery()
    {
        hasDelivery = false;
        deliveryRequest = null;
    }

    public static DeliveryRequest FromJob(Job job)
    {
        var requests = new List<DeliveryRequest.Request>();
        if (job.yellowSeeds > 0)
            requests.Add(new DeliveryRequest.Request
            {
                goodsType = Goods.GoodsType.YellowFlower,
                need = job.yellowSeeds,
                satisfied = 0
            });
        if (job.redSeeds > 0)
            requests.Add(new DeliveryRequest.Request
            {
                goodsType = Goods.GoodsType.RedBerry,
                need = job.redSeeds,
                satisfied = 0
            });
        if (job.waterSeeds > 0)
            requests.Add(new DeliveryRequest.Request
            {
                goodsType = Goods.GoodsType.WaterPlant,
                need = job.waterSeeds,
                satisfied = 0
            });

        return new DeliveryRequest
        {
            deadline = job.time,
            requests = requests.ToArray()
        };
    }

    public void StartDelivery(DeliveryRequest newDelivery)
    {
        deliveryRequest = newDelivery;
        hasDelivery = true;

        TruckMover.Instance.StartDeliver(); // TODO: Fix circular dependency to TruckMover
    }
}