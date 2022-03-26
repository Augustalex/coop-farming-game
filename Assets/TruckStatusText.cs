using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckStatusText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        var truckState = TruckMover.Instance.GetState();
        TruckStatusChanged(ToTruckStatus(truckState));
    }

    public static TruckStatus ToTruckStatus(TruckMover.TruckState truckState)
    {
        if (truckState == TruckMover.TruckState.DrivingFromStart)
        {
            return TruckStatus.OnWay;
        }
        else if (truckState == TruckMover.TruckState.DrivingToStop)
        {
            return TruckStatus.Moving;
        }
        else if (truckState == TruckMover.TruckState.Waiting)
        {
            return TruckStatus.Waiting;
        }
        else return TruckStatus.RequestingDelivery;
    }

    public enum TruckStatus
    {
        OnWay,
        Moving,
        Waiting,
        RequestingDelivery
    }

    public TruckStatus _truckStatus;
    private TMP_Text _text;

    public void TruckStatusChanged(TruckStatus newStatus)
    {
        _truckStatus = newStatus;

        UpdateText();
    }

    private void UpdateText()
    {
        if (_truckStatus == TruckStatus.OnWay)
        {
            _text.text = "TRUCK ON ITS WAY";
        }
        if (_truckStatus == TruckStatus.Moving)
        {
            _text.text = "TRUCK MOVING";
        }
        else if (_truckStatus == TruckStatus.Waiting)
        {
            _text.text = "";
        }
        else if (_truckStatus == TruckStatus.RequestingDelivery)
        {
            _text.text = "";
        }
    }
}
