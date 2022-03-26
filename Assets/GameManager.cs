using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CinemachineVirtualCamera mainVirtualCamera;
    
    public GameSettings gameSettings;
    public int money = 0;
    public int jobsDone = 0;

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
}
