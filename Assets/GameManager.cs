using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CinemachineVirtualCamera mainVirtualCamera;

    public GameSettings gameSettings;
    public int prestige = 0;
    public int money = 0;
    public int jobsDone = 0;
    public int starLevel = 0;
    public int fails = 0;

    public bool cheatUpgrade;
    public bool failNow;

    public event Action<int> PrestigeChanged;
    public event Action<int> StarLevelChanged;
    public event Action<int> FailAttemptsChanged;

    private void Update()
    {
        if (cheatUpgrade)
        {
            cheatUpgrade = false;
            UpPrestige(10);
        }

        if (failNow)
        {
            failNow = false;
            RegisterFail();
        }
    }

    public void FocusCamera(CinemachineVirtualCamera virtualCamera)
    {
        mainVirtualCamera.Priority = 10;

        virtualCamera.Priority = 20;
    }

    public void ResetCameraFocus()
    {
        var cameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var cinemachineVirtualCamera in cameras)
        {
            cinemachineVirtualCamera.Priority = 10;
        }

        mainVirtualCamera.Priority = 20;
    }

    public Vector3 GetCameraPosition()
    {
        return mainVirtualCamera.transform.position;
    }

    public void UseMoney(int cost)
    {
        money -= cost;
    }

    public void UpPrestige(int count)
    {
        prestige += count;

        UpdateStarLevel();

        PrestigeChanged?.Invoke(prestige);
    }

    private void UpdateStarLevel()
    {
        if (prestige >= 100)
        {
            starLevel = 4;
        }
        else if (prestige >= 60)
        {
            starLevel = 3;
        }
        else if (prestige >= 10)
        {
            starLevel = 2;
        }
        else if (prestige >= 2)
        {
            starLevel = 1;
        }
        else
        {
            starLevel = 0;
        }

        StarLevelChanged?.Invoke(starLevel);
    }

    public void DownPrestige(int count)
    {
        prestige = Mathf.Max(prestige - count, 0);

        UpdateStarLevel();

        PrestigeChanged?.Invoke(prestige);
    }

    public void RegisterFail()
    {
        fails += 1;
        FailAttemptsChanged?.Invoke(fails);
    }

    public void SpawnCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}