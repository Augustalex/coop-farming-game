using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[Obsolete("Not used any more", true)]
public class RedSeed : MonoBehaviour
{
    private SeedItem _seedItem;

    private void Start()
    {
        _seedItem = GetComponent<SeedItem>();
        _seedItem.seedTemplate = AssetLibrary.Instance.redSeedTemplate;
        
        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    void OnUse(Vector3 highlightPosition)
    {
        var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
            .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var hit = hits[0];
            if (hit.collider.CompareTag("Soil"))
            {
                var soilBlock = hit.collider.GetComponent<SoilBlock>();
                if (soilBlock.IsFree())
                {
                    _seedItem.OnUseOnSoil(soilBlock);
                }
            }
        }
    }
}