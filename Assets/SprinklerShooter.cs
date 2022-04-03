using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SprinklerShooter : MonoBehaviour
{
    private const float MaxWater = 180f;
    private float _water = 120f;
    private bool _running;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (_running)
        {
            if (_water < 0)
            {
                StopSprinkler();
            }
            else
            {
                _water -= Time.deltaTime;
            }
        }

        if (!_particleSystem.isPlaying && _water > 0)
        {
            StartSprinkler();
        }
    }

    private void StartSprinkler()
    {
        _running = true;
        _particleSystem.Play();
    }

    private void StopSprinkler()
    {
        _running = false;
        _particleSystem.Stop();
    }

    public void Water(float waterPercentage)
    {
        _water += MaxWater * .25f * waterPercentage;
    }

    public float GetMaxWaterTime()
    {
        return MaxWater;
    }

    public float GetWaterTime()
    {
        return _water;
    }
}