using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public event Action<Vector3> UseItem;
    public event Action Dropped;
    public event Action Grabbed;
    
    public enum ItemType
    {
        SeedSack,
        WaterCan,
        Other
    }

    public ItemType itemType;
    private PlayerGrabber _grabber;
    
    public void Use(Vector3 highlightPosition)
    {
        UseItem?.Invoke(highlightPosition);
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
        _grabber = playerGrabber;
        Grabbed?.Invoke();
    }

    public void WasDropped()
    {
        Dropped?.Invoke();
    }
}