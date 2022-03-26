using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class SeedItem : MonoBehaviour
{
    public GameObject seedTemplate;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    private void OnUse(Vector3 highlightPosition)
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
                    OnUseOnSoil(soilBlock);
                }
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