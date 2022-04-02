using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hose : MonoBehaviour
{
    private bool _on = true;

    void Start()
    {
        
    }

    void Update()
    {
        // if (_on)
        // {
        //     var hits = Physics.OverlapBox(transform.position, new Vector3(
        //         3f, 3f, 12f
        //     )).Where(hit => hit.GetComponent<SeedGrowth>());
        //     foreach (var hit in hits)
        //     {
        //         var seedGrowth = hit.GetComponent<SeedGrowth>();
        //         if (seedGrowth)
        //         {
        //             seedGrowth.Water();
        //         }
        //     }
        // }
    }
}