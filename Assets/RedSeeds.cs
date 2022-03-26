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
        var hits = Physics.OverlapSphere(transform.position, 4f).Where(hit => hit.CompareTag("Element") || hit.CompareTag("Goods"));

        return hits.All(hit => hit.GetComponent<PlantIdentifier>()?.plantType != PlantIdentifier.PlantType.YellowFlower) &&
               hits.Count(hit =>
                   hit.GetComponent<PlantIdentifier>()?.plantType == PlantIdentifier.PlantType.YellowFlower) < 3;
    }
}
