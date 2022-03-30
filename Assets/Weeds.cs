using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class Weeds : MonoBehaviour
{
    private float _spreadCooldown = 10;

    void Update()
    {
        if (_spreadCooldown > 0)
        {
            _spreadCooldown -= Time.deltaTime;
        }
        else
        {
            _spreadCooldown = 10;
            Spread();
        }
    }

    private void Spread()
    {
        var rawHits = Physics.OverlapSphere(transform.position, 2f);
        foreach (var hit in rawHits)
        {
            if (hit.CompareTag("Soil"))
            {
                var soilBlock = hit.GetComponent<SoilBlock>();
                if (soilBlock.HasSeed())
                {
                    soilBlock.KillSeed();
                    soilBlock.AttachWeeds(AssetLibrary.Instance.weedsTemplate);

                    return; // Spread only to 1
                }
            }
        }
    }
}
