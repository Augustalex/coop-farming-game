using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class TruckMover : MonoSingleton<TruckMover>
{
    public GameObject truck;
    public Transform start;
    public Transform end;
    public Transform loadingSpot;
    private TruckState _state;
    private float _waitingCooldown;
    private float _progress;

    public enum TruckState
    {
        Waiting,
        DrivingFromStart,
        Loading,
        DrivingToStop
    }

    void Start()
    {
        truck.transform.position = start.position;

        _state = TruckState.Waiting;
        _waitingCooldown = GameManager.Instance.gameSettings.truckStartDelay;
    }

    public void StartDeliver()
    {
        _state = TruckState.DrivingFromStart;

        Sounds.Instance.PlayTruckMovingSound(truck.transform.position);
    }

    void Update()
    {
        if (_state == TruckState.Waiting)
        {
        }
        else if (_state == TruckState.DrivingFromStart)
        {
            var loadingPosition = loadingSpot.position;
            var startPosition = start.position;
            _progress += Time.deltaTime / GameManager.Instance.gameSettings.truckTimeToMove;
            truck.transform.position = Vector3.Lerp(startPosition, loadingPosition, _progress);

            if (_progress >= 1f)
            {
                _state = TruckState.Loading;
                Sounds.Instance.PlayTruckHornSound(truck.transform.position);
            }
        }
        else if (_state == TruckState.Loading)
        {
            var timeLeftOfDelivery =
                Mathf.Clamp(DeliveryManager.Instance.deliveryRequest.deadline - Time.time, 0f, 999f);
            if (timeLeftOfDelivery == 0f)
            {
                _state = TruckState.DrivingToStop;

                _progress = 0f;

                Sounds.Instance.PlayTruckHornSound(truck.transform.position);
            }
        }
        else if (_state == TruckState.DrivingToStop)
        {
            var endPosition = end.position;
            var startPosition = loadingSpot.position;
            _progress += Time.deltaTime / GameManager.Instance.gameSettings.truckTimeToMoveAway;
            truck.transform.position = Vector3.Lerp(startPosition, endPosition, _progress);

            if (_progress >= 1f)
            {
                _state = TruckState.Waiting;

                _waitingCooldown = GameManager.Instance.gameSettings.truckTimeBetweenDeliveries;
                _progress = 0f;
            }
        }
    }

    void Update2()
    {
        if (_state == TruckState.Waiting)
        {
            _waitingCooldown -= Time.deltaTime;
            if (_waitingCooldown < 0f)
            {
                _state = TruckState.DrivingFromStart;

                var deliveryManager = DeliveryManager.Instance;
                deliveryManager.deliveryRequest = new DeliveryRequest
                {
                    requests = new[]
                    {
                        new DeliveryRequest.Request
                        {
                            goodsType = Goods.GoodsType.Plant,
                            need = 10,
                            satisfied = 0
                        },
                    },
                    deadline = Time.time + GameManager.Instance.gameSettings.deliveryTime
                };
            }
        }
        else if (_state == TruckState.DrivingFromStart)
        {
            var loadingPosition = loadingSpot.position;
            var startPosition = start.position;
            _progress += Time.deltaTime / GameManager.Instance.gameSettings.truckTimeToMove;
            truck.transform.position = Vector3.Lerp(startPosition, loadingPosition, _progress);

            if (_progress >= 1f)
            {
                _state = TruckState.Loading;
                var deliveryManager = DeliveryManager.Instance;
                deliveryManager.deliveryRequest = new DeliveryRequest
                {
                    requests = new[]
                    {
                        new DeliveryRequest.Request
                        {
                            goodsType = Goods.GoodsType.Plant,
                            need = 10,
                            satisfied = 0
                        },
                    },
                    deadline = Time.time + GameManager.Instance.gameSettings.deliveryTime
                };
                deliveryManager.hasDelivery = true;
            }
        }
        else if (_state == TruckState.Loading)
        {
            // Waiting for delivery to run out of time or be done
        }
        else if (_state == TruckState.DrivingToStop)
        {
            var endPosition = end.position;
            var startPosition = loadingSpot.position;
            _progress += Time.deltaTime / GameManager.Instance.gameSettings.truckTimeToMoveAway;
            truck.transform.position = Vector3.Lerp(startPosition, endPosition, _progress);

            if (_progress >= 1f)
            {
                _state = TruckState.Waiting;

                _waitingCooldown = GameManager.Instance.gameSettings.truckTimeBetweenDeliveries;
                _progress = 0f;
            }
        }
    }

    public void DeliveryEnded()
    {
        if (_state == TruckState.Loading)
        {
            _state = TruckState.DrivingToStop;
            _progress = 0f;
        }
    }

    public TruckState GetState()
    {
        return _state;
    }

    public bool MovingToEnd()
    {
        return _state == TruckState.DrivingToStop;
    }

    public bool IsLoading()
    {
        return _state == TruckState.Loading;
    }
}