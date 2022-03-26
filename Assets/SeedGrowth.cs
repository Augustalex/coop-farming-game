using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class SeedGrowth : MonoBehaviour
{
    public GameObject bushTemplate;
    public GameObject plantTemplate;
    public float dryDeathTime = 60f;
    public float waterChargeTime = 60f;
    public float timeToGrow = 60f;

    public float waterLevels = 4f;
    public float healthLevels = 4f;

    public bool hasNutrients = false;

    public Func<bool> CanGrowFunc;

    public event Action<Plant> GrownUp;
    public event Action Died;
    public event Action NoWater;
    public event Action<LevelData> LevelsUpdated;

    public struct LevelData
    {
        public int healthLevel;
        public int waterLevel;
        public WaterLevelIndicator.WaterLevelIndicatorState waterState;
    }

    private const float WaterLevelsRefillAmount = 2.5f;

    private float _growthTime = 0f;
    private float _waterLevel = 0f;
    private bool _grownUp;

    public void Update()
    {
        if (_grownUp) return;

        if (_growthTime >= timeToGrow)
        {
            if (CanGrow())
            {
                MakeGrownUp();
            }
            else
            {
                Die();
            }
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
                Die();
            }
            else
            {
                _waterLevel -= Time.deltaTime;

                LevelsUpdated?.Invoke(GetLevelData());
            }
        }
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
                : WaterLevelIndicator.WaterLevelIndicatorState.Dry
        };
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
        
        _waterLevel = Mathf.Clamp(_waterLevel + ((waterChargeTime / waterLevels) * WaterLevelsRefillAmount), 0,
            waterChargeTime);
    }

    public int GetHealthLevel()
    {
        return Mathf.RoundToInt((_growthTime / timeToGrow) * healthLevels);
    }

    public int GetWaterLevel()
    {
        return Mathf.RoundToInt(Mathf.Abs(_waterLevel) / (waterChargeTime / waterLevels));
    }

    public bool IsWet()
    {
        return _waterLevel > 0;
    }
}