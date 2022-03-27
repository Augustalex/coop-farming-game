using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RodSpawner))]
public class RodSeedSource : MonoBehaviour
{
    private RodSpawner _rodSpawner;
    private RiverSeedGrowth _riverSeedGrowth;

    void Start()
    {
        _rodSpawner = GetComponent<RodSpawner>();
        _riverSeedGrowth = GetComponent<RiverSeedGrowth>();

        if (_riverSeedGrowth)
        {
            _riverSeedGrowth.CanGrowFunc = _rodSpawner.CanGrow;
            _riverSeedGrowth.Grown += OnGrown;
        }
    }

    private void OnGrown()
    {
        if (!_rodSpawner.CanGrow()) return; // TODO: A bit too defensive perhaps? This should be checked before calling this function - always.

        _rodSpawner.Grow();
    }

    public void Pick()
    {
        if (_rodSpawner.CanBePicked())
        {
            _rodSpawner.Pick();
        }
        else
        {
            if (_riverSeedGrowth)
            {
                _riverSeedGrowth.Die();
            }
            Destroy(gameObject);
        }
    }
}