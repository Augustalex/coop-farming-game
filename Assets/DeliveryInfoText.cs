using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class DeliveryInfoText : MonoBehaviour
{
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        var deliveryManager = DeliveryManager.Instance;
        if (!deliveryManager.hasDelivery)
        {
            _text.text = "No ongoing delivery";
        }
        else if (TruckStatusText.ToTruckStatus(TruckMover.Instance.GetState()) == TruckStatusText.TruckStatus.OnWay)
        {
            var request = deliveryManager.deliveryRequest;

            var text = $"Next delivery";

            foreach (var requestRequest in request.requests)
            {
                text += $"\n{GoodsName(requestRequest.goodsType)} {requestRequest.need}pc";
            }

            _text.text = text;
        }
        else
        {
            var request = deliveryManager.deliveryRequest;

            var text = $"Requesting delivery {ToTimerText(request.deadline)}";

            foreach (var requestRequest in request.requests)
            {
                text += $"\n{GoodsName(requestRequest.goodsType)} {requestRequest.satisfied}/{requestRequest.need}";
            }

            _text.text = text;
        }
    }

    private string ToTimerText(float requestDeadline)
    {
        var timeLeft = Mathf.Floor(Mathf.Clamp(requestDeadline - Time.time, 0, 999f));
        return $"{timeLeft}s";
    }

    private string GoodsName(Goods.GoodsType goodsType)
    {
        if (goodsType == Goods.GoodsType.Plant) return "Plant";
        if (goodsType == Goods.GoodsType.YellowFlower) return "Yellow flower";
        if (goodsType == Goods.GoodsType.RedBerry) return "Red berry";
        if (goodsType == Goods.GoodsType.WaterPlant) return "Water plant";
        else return "Unknown";
    }
}