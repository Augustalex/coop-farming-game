using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGrowth : MonoBehaviour
{
    public GrowthStage[] stages;

    private int _stage;
    private float _progress;

    public event Action Died;

    [System.Serializable]
    public struct GrowthStage
    {
        public GameObject item;
        public float growTime;
    }

    private void Start()
    {
        foreach (var growthStage in stages)
        {
            growthStage.item.SetActive(false);
        }

        stages[_stage].item.SetActive(true);
        _progress = 0;
    }

    void Update()
    {
        if (_stage < stages.Length)
        {
            // Auto grow
            // var currentStage = stages[_stage];
            // if (_progress >= 1f)
            // {
            //     currentStage.item.SetActive(false);
            //
            //     _progress = 0;
            //     _stage += 1;
            //
            //     stages[_stage].item.SetActive(true);
            // }
            // else
            // {
            //     _progress += Time.deltaTime / currentStage.growTime;
            // }
        }

        // Wither
        var currentStage = stages[_stage];
        if (_progress >= 1f)
        {
            currentStage.item.SetActive(false);

            _progress = 0;
            _stage -= 1;
            GameManager.Instance.DownPrestige(1);

            if (_stage == 0)
            {
                Die();
            }
            else
            {
                stages[_stage].item.SetActive(true);
            }
        }
        else
        {
            _progress += Time.deltaTime / currentStage.growTime;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Died?.Invoke();
    }

    public void Water()
    {
        _progress = 0f;
        if (_stage >= stages.Length - 1) return;

        var currentStage = stages[_stage];
        currentStage.item.SetActive(false);
        _progress = 0f;

        _stage += 1;
        stages[_stage].item.SetActive(true);
        GameManager.Instance.UpPrestige(1);
    }
}