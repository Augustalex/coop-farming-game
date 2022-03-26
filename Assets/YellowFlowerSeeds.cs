using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YellowFlowerSeeds : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SeedGrowth>().CanGrowFunc = CanGrow;
    }

    private bool CanGrow()
    {
        return true;
    }
}