using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class RiverSeedGrowth : MonoBehaviour
{
    public float timeToGrow = 10f;

    public float healthLevels = 4f;

    public Func<bool> CanGrowFunc;

    public event Action Grown;
    public event Action Died;
    public event Action<RiverLevelData> LevelsUpdated;

    public struct RiverLevelData
    {
        public int healthLevel;
        public float maxHealthLevels;
    }

    private float _growthTime = 0f;
    private int _speedNutrients = 0;

    public void Update()
    {
        if (_growthTime >= GetTimeToGrow())
        {
            _growthTime = 0;
            if (CanGrow())
            {
                MakeGrownUp();
            }
        }
        else
        {
            _growthTime += Time.deltaTime;
            LevelsUpdated?.Invoke(GetLevelData());
        }
    }

    public void MakeGrownUp()
    {
        Sounds.Instance.PlayGrowPlantSound(transform.position);

        LevelsUpdated?.Invoke(GetLevelData());
        Grown?.Invoke();
    }

    public RiverLevelData GetLevelData()
    {
        return new RiverLevelData
        {
            healthLevel = GetHealthLevel(),
            maxHealthLevels = GetHealthLevels()
        };
    }

    private float GetHealthLevels()
    {
        if (_speedNutrients > 0) return Mathf.Round(healthLevels * Mathf.Pow(.75f, _speedNutrients));
        return healthLevels;
    }

    public void Die()
    {
        Died?.Invoke();
        Destroy(gameObject);
    }

    public bool CanGrow()
    {
        return CanGrowFunc.Invoke();
    }

    public int GetHealthLevel()
    {
        return Mathf.RoundToInt((_growthTime / GetTimeToGrow()) * GetHealthLevels());
    }

    public float GetTimeToGrow()
    {
        if (_speedNutrients > 0) return timeToGrow * Mathf.Pow(.75f, _speedNutrients);
        return timeToGrow;
    }

    public void AddSpeedNutrient()
    {
        _speedNutrients += 1;
    }
}