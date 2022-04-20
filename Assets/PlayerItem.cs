using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public bool magnetic;

    public event Action<Vector3> UseItem;
    public event Action Dropped;
    public event Action Grabbed;
    public event Action<Vector3> Provoked;

    public enum ItemType
    {
        SeedSack,
        WaterCan,
        Other
    }

    public ItemType itemType;
    private PlayerGrabber _grabber;

    public enum LifterIdentifier
    {
        Highlight,
        Grabber
    }

    public struct LiftUpData
    {
        public float heightOffset;
        public LifterIdentifier lifter;
    }

    private List<LiftUpData> _lifters = new List<LiftUpData>();
    private Rigidbody _body;

    void Start()
    {
        _body = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (IsGrabbed()) return;
        
        var currentPosition = transform.position;
        var liftHeightOffset = GetLiftHeightOffset();
        transform.position = new Vector3(
            currentPosition.x,
            liftHeightOffset > 0 ? liftHeightOffset : currentPosition.y,
            currentPosition.z
        );
    }

    public void LiftUp(LifterIdentifier identifier, float heightOffset)
    {
        if (_lifters.Count == 0)
        {
            _body.isKinematic = true;
        }

        _lifters.Add(new LiftUpData {heightOffset = heightOffset, lifter = identifier});
    }

    public void StopLifting(LifterIdentifier identifier)
    {
        if (_lifters.Any(data => data.lifter == identifier))
        {
            _lifters = _lifters.Where(data => data.lifter != identifier).ToList();
        }

        if (_lifters.Count == 0)
        {
            _body.isKinematic = false;
        }
    }

    public float GetLiftHeightOffset()
    {
        if (_lifters.Count > 0)
        {
            return _lifters[_lifters.Count - 1].heightOffset;
        }
        else
        {
            return 0f;
        }
    }

    public void Use(Vector3 highlightPosition)
    {
        UseItem?.Invoke(highlightPosition);
    }

    public void Provoke(Vector3 highlightPosition)
    {
        Provoked?.Invoke(highlightPosition);
    }

    public bool IsGrabbed()
    {
        return _grabber != null;
    }

    public void Steal()
    {
        _grabber.StealGrabbedItem();
        _grabber = null;
    }

    public void GrabbedBy(PlayerGrabber playerGrabber)
    {
        LiftUp(LifterIdentifier.Grabber, 2.5f);

        _grabber = playerGrabber;
        Grabbed?.Invoke();
    }

    public void WasDropped()
    {
        StopLifting(LifterIdentifier.Grabber);

        _grabber = null;
        Dropped?.Invoke();
    }

    public bool IsMagnetic()
    {
        return magnetic;
    }
}