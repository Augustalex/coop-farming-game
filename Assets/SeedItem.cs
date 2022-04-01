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

    public AudioClip pickupSound;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
        // GetComponent<PlayerItem>().Grabbed += OnPickedUp;

        _useOnSoil = GetComponent<UseOnSoil>();
    }

    // private void OnPickedUp()
    // {
    //     if (!pickupSound)
    //     {
    //         Sounds.Instance.PlayPickupItemSound(transform.position);
    //     }
    //     else
    //     {
    //         Sounds.Instance.PlaySound(pickupSound, transform.position, .5f);
    //     }
    // }

    private void OnUse(Vector3 highlightPosition)
    {
        var soil = _useOnSoil.HoverOnSoil(highlightPosition);
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
            Sounds.Instance.PlayPlaceTileSound(soilBlock.transform.position);
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