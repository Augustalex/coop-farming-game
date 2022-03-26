using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plant : MonoBehaviour
{
    public event Action Harvested;

    private PlantIdentifier _plantIdentifier;
    private bool _inSoil;

    private void Awake()
    {
        _plantIdentifier = GetComponent<PlantIdentifier>();
    }

    public void Grabbed()
    {
        if (_inSoil)
        {
            Harvest();
        }
    }

    public void Harvest()
    {
        _inSoil = false;
        Harvested?.Invoke();
    }

    public bool CompareToPlant(Plant plant)
    {
        return plant._plantIdentifier.CompareToPlant(_plantIdentifier);
    }

    public PlantIdentifier GetPlantIdentifier()
    {
        return _plantIdentifier;
    }

    public void SetAsInSoil()
    {
        _inSoil = true;
    }

    public bool InSoil()
    {
        return _inSoil;
    }
}