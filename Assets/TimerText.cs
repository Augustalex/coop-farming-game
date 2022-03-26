using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour
{
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        var deliveryManager = DeliveryManager.Instance;
        if (deliveryManager.hasDelivery && TimeLeft(deliveryManager.deliveryRequest.deadline) < 11f)
        {
            var request = deliveryManager.deliveryRequest;

            _text.text = $"{ToTimerText(request.deadline)}";
        }
        else
        {
            _text.text = "";
        }
    }

    private string ToTimerText(float requestDeadline)
    {
        return $"{TimeLeft(requestDeadline)}s";
    }

    private float TimeLeft(float deadline)
    {
        var timeLeft = Mathf.Clamp(deadline - Time.time, 0, 999f);

        return Mathf.Floor(timeLeft);
    }
}