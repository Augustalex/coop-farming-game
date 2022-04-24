using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public bool liftOnGrab = true;

    public bool magnetic;

    public event Action<Vector3> UseItem;
    public event Action Dropped;
    public event Action Grabbed;
    public event Action Escaped;
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
        Grabber,
        NonPlayer
    }

    public struct LiftUpData
    {
        public float heightOffset;
        public LifterIdentifier lifter;
    }

    private List<LiftUpData> _lifters = new List<LiftUpData>();
    private Rigidbody _body;
    private bool _frozen;
    private GameObject _grabbedByNonPlayer;

    void Start()
    {
        _body = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (!liftOnGrab && IsGrabbed()) return;

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
        return _grabber != null || _grabbedByNonPlayer != null;
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

    public void GrabbedByNonPlayer(GameObject nonPlayer)
    {
        _grabbedByNonPlayer = nonPlayer;
        LiftUp(LifterIdentifier.NonPlayer, 0f);

        Grabbed?.Invoke();
    }

    public void Escape()
    {
        DropAndStopLifting();
        Escaped?.Invoke();   
    }

    public void WasDropped()
    {
        DropAndStopLifting();
        Dropped?.Invoke();
    }

    private void DropAndStopLifting()
    {
        if (_grabbedByNonPlayer != null)
        {
            _grabbedByNonPlayer = null;
            StopLifting(LifterIdentifier.NonPlayer);
        }

        if (_grabber != null)
        {
            _grabber = null;
            StopLifting(LifterIdentifier.Grabber);
        }
    }

    public bool IsMagnetic()
    {
        return magnetic;
    }
}