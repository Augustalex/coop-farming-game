using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedSeeds : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SeedGrowth>().CanGrowFunc = CanGrow;
    }

    private bool CanGrow()
    {
        var hits = Physics.OverlapBox(transform.position, Vector3.one * 1.4f).Where(hit => hit.CompareTag("Element"));

        return hits.All(hit =>
            hit.GetComponent<PlantIdentifier>()?.plantType != PlantIdentifier.PlantType.YellowFlower);
    }
}