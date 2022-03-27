using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class JobPaperRoot : MonoBehaviour
{
    private JobPaperItemGenerator _itemGenerator;
    private Job _job;
    private GameManager _gameManager;

    public bool waterJob = false;

    void Awake()
    {
        _itemGenerator = GetComponentInChildren<JobPaperItemGenerator>();
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Time.time >= (_job.time - 50f))
        {
            SetupWithRandomJob();
        }
    }

    public void SetupForJob(Job job)
    {
        _job = job;
        _itemGenerator.Generate(job);
    }

    public void SetupWithRandomJob()
    {
        if (waterJob)
        {
            SetupForJob(
                new Job
                {
                    rodSeeds = Random.Range(1, 6) * 10,
                    time = Deadline(),
                }
            );
        }
        else
        {
            var redSeedCount = Random.value < RedBerryChance() ? Random.Range(0, GetUpperRange()) * 2 : 0;
            var waterSeeds = Random.value < WaterPlantChance() ? Random.Range(0, GetUpperRange()) * 2 : 0;
            var yellowSeeds = redSeedCount == 0 && waterSeeds == 0
                ? Random.Range(1, GetUpperRange()) * 2
                : Random.Range(0, GetUpperRange()) * 2;
            SetupForJob(
                new Job
                {
                    redSeeds = redSeedCount,
                    waterSeeds = waterSeeds,
                    yellowSeeds = yellowSeeds,
                    time = Deadline(),
                }
            );
        }
    }

    public float Deadline()
    {
        var baseTime = Time.time + 60;
        var range = Random.Range(MinTimeRange(), MaxTimeRange());
        return baseTime + range * 60 * 2;
    }

    private int MaxTimeRange()
    {
        if (_gameManager.jobsDone < 2) return 6;
        if (_gameManager.jobsDone < 6) return 3;
        return 2;
    }

    private int MinTimeRange()
    {
        if (_gameManager.jobsDone < 4) return 2;
        return 1;
    }

    public float RedBerryChance()
    {
        if (_gameManager.jobsDone < 2) return 0;
        if (_gameManager.jobsDone < 4) return 0.5f;
        return 0.7f;
    }

    public float WaterPlantChance()
    {
        if (_gameManager.jobsDone < 4) return 0;
        if (_gameManager.jobsDone < 6) return 0.4f;
        return 0.7f;
    }

    public int GetUpperRange()
    {
        var jobsDone = _gameManager.jobsDone;

        if (jobsDone < 3) return 3;
        if (jobsDone < 6) return 6;
        if (jobsDone < 12) return 12;
        if (jobsDone < 24) return 16;
        return 21;
    }

    public Job GetJob()
    {
        return _job;
    }
}