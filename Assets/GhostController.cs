using System;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject goodGhost;
    public GameObject badGhost;

    public void CanPlace()
    {
        goodGhost.SetActive(true);
        badGhost.SetActive(false);
    }

    public void CanNotPlace()
    {
        goodGhost.SetActive(false);
        badGhost.SetActive(true);
    }
}