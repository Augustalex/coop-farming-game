using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class WaterBobSeeds : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SeedGrowth>().CanGrowFunc = CanGrow;
    }

    private bool CanGrow()
    {
        var hits = Physics.OverlapSphere(transform.position, 3f).Count(hit => hit.GetComponent<WaterSource>());

        return hits > 0;
    }
}
