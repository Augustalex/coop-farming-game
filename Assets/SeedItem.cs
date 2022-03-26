using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(UseOnSoil))]
public class SeedItem : MonoBehaviour
{
    public GameObject seedTemplate;
    private UseOnSoil _useOnSoil;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;

        _useOnSoil = GetComponent<UseOnSoil>();
    }

    private void OnUse(Vector3 highlightPosition)
    {
        var soil = _useOnSoil.HoveringSoil(highlightPosition);
        if (soil)
        {
            var soilBlock = soil.GetComponent<SoilBlock>();
            if (soilBlock.IsFree())
            {
                OnUseOnSoil(soilBlock);
            }
        }
    }

    public void OnUseOnSoil(SoilBlock soilBlock)
    {
        if (soilBlock.IsFree())
        {
            SeedSoil(soilBlock);
        }
    }

    private void SeedSoil(SoilBlock soil)
    {
        soil.SeedWithTemplate(seedTemplate);

        Destroy(gameObject);
    }

    public bool CompareToSeedItem(SeedItem item)
    {
        return seedTemplate == item.seedTemplate;
    }
}