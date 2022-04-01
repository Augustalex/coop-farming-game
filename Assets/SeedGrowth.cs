using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    public GameObject seedItemTemplate;
    public GameObject plantTemplate;
    public float dryDeathTime = 60f;
    public float waterChargeTime = 60f;
    public float timeToGrow = 60f;

    public float waterLevels = 4f;
    public float healthLevels = 4f;

    public Func<bool> CanGrowFunc;

    public event Action<Plant> GrownUp;
    public event Action Died;
    public event Action NoWater;
    public event Action Corrupted; // TODO: Deprecate
    public event Action WasHurt;
    public event Action<LevelData> LevelsUpdated;

    public struct LevelData
    {
        public int healthLevel;
        public int waterLevel;
        public WaterLevelIndicator.WaterLevelIndicatorState waterState;
        public float maxWaterLevels;
        public float maxHealthLevels;
    }

    private const float WaterLevelsRefillAmount = 2.5f;

    private float _growthTime = 0f;
    private float _waterLevel = 0f;
    private bool _grownUp;

    // private bool _hasNutrients = false;
    // private bool _hasSpeedNutrients = false;

    private int _waterNutrients = 0;
    private int _speedNutrients = 0;
    private float _checkCooldown = 0;

    public void Update()
    {
        if (_grownUp) return;

        if (_growthTime > 2)
        {
            if (_checkCooldown > 0)
            {
                _checkCooldown -= Time.deltaTime;
            }
            else
            {
                _checkCooldown = 2f;
                if (!CanGrow())
                {
                    Corrupt();
                }
            }
        }

        if (_growthTime >= GetTimeToGrow())
        {
            // if (CanGrow())
            // {
            MakeGrownUp();
            // }
            // else
            // {
            // Die();
            // }
        }
        else
        {
            if (_waterLevel > 0f)
            {
                _growthTime += Time.deltaTime;
            }
            else
            {
                NoWater?.Invoke();
            }

            if (_waterLevel < -dryDeathTime)
            {
                Corrupt();
                // Die(); 
            }
            else
            {
                _waterLevel -= Time.deltaTime;

                LevelsUpdated?.Invoke(GetLevelData());
            }
        }
    }

    private void Corrupt()
    {
        Corrupted?.Invoke();
        Die();

        var newSeedItem = Instantiate(seedItemTemplate, transform.position + Vector3.up, Quaternion.identity, null);
        var va = newSeedItem.GetComponent<CubeVoiceActor>();
        if (va)
        {
            va.OnRelocated();
        }

        var wild = newSeedItem.GetComponent<WildCube>();
        wild.WantsWater();
    }

    public void MakeGrownUp()
    {
        _grownUp = true;
        Destroy(gameObject, .1f);

        var plant = Instantiate(plantTemplate);
        plant.transform.position = transform.position + Vector3.up * 2f;

        Sounds.Instance.PlayGrowPlantSound(transform.position);

        GrownUp?.Invoke(plant.GetComponent<Plant>()); // TODO: Is it in GetComponentChildren perhaps?
    }

    public LevelData GetLevelData()
    {
        return new LevelData
        {
            healthLevel = GetHealthLevel(),
            waterLevel = GetWaterLevel(),
            waterState = IsWet()
                ? WaterLevelIndicator.WaterLevelIndicatorState.Wet
                : WaterLevelIndicator.WaterLevelIndicatorState.Dry,
            maxWaterLevels = GetWaterLevels(),
            maxHealthLevels = GetHealthLevels()
        };
    }

    private float GetHealthLevels()
    {
        // if (_hasSpeedNutrients) return Mathf.Round(healthLevels * .75f);
        if (_speedNutrients > 0) return Mathf.Round(healthLevels * Mathf.Pow(.75f, _speedNutrients));
        return healthLevels;
    }

    public void Kill()
    {
        Die();
    }

    private void Die()
    {
        Died?.Invoke();
        Destroy(gameObject);
    }

    public bool CanGrow()
    {
        return CanGrowFunc.Invoke();
    }

    public void Water()
    {
        if (_waterLevel < 0) _waterLevel = 0;

        _waterLevel = Mathf.Clamp(_waterLevel + ((GetWaterChargeTime() / GetWaterLevels()) * WaterLevelsRefillAmount),
            0,
            GetWaterChargeTime());
    }

    public int GetHealthLevel()
    {
        return Mathf.RoundToInt((_growthTime / GetTimeToGrow()) * GetHealthLevels());
    }

    public float GrowthTimeOfOneHealthLevel()
    {
        return GetTimeToGrow() / GetHealthLevels();
    }

    public float GetTimeToGrow()
    {
        // if (_hasSpeedNutrients) return timeToGrow * .75f;
        if (_speedNutrients > 0) return timeToGrow * Mathf.Pow(.75f, _speedNutrients);
        return timeToGrow;
    }

    public int GetWaterLevel()
    {
        return Mathf.RoundToInt(Mathf.Abs(_waterLevel) / (GetWaterChargeTime() / GetWaterLevels()));
    }

    public float GetWaterChargeTime()
    {
        // if (_hasNutrients) return waterChargeTime * 2f;
        if (_waterNutrients > 0) return waterChargeTime + ((waterChargeTime / waterLevels) * (_waterNutrients * 2f));
        return waterChargeTime;
    }

    public float GetWaterLevels()
    {
        // if (_hasNutrients) return waterLevels * 2f;
        if (_waterNutrients > 0) return waterLevels + (_waterNutrients * 2f);
        return waterLevels;
    }

    public bool IsWet()
    {
        return _waterLevel > 0;
    }

    public void AddNutrient()
    {
        _waterNutrients += 1;
        // _hasNutrients = true;
    }

    public void AddSpeedNutrient()
    {
        _speedNutrients += 1;
        // _hasSpeedNutrients = true;
    }

    public void Hurt()
    {
        _growthTime = Mathf.Clamp(_growthTime - GrowthTimeOfOneHealthLevel(), 0, GetTimeToGrow());
        WasHurt?.Invoke();
    }
}