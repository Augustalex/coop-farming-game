using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
using UnityEngine;

public class IntroManager : MonoSingleton<IntroManager>
{
    public GameObject introPaper;
    public CinemachineVirtualCamera startCamera;
    public bool isIntro = true;

    public void EndIntro()
    {
        isIntro = false;
        startCamera.Priority = 0;
        Destroy(introPaper);

        GameManager.Instance.StartGame();
    }
}