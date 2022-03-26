using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(CountData))]
public class NutrientBag : MonoBehaviour
{
    private CountData _countData;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
        _countData = GetComponent<CountData>();
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (!gameObject) return;

        var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
            .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var hit = hits[0];
            if (hit.collider.CompareTag("Soil"))
            {
                var soil = hit.collider.gameObject;
                var soilBlock = soil.GetComponent<SoilBlock>();
                if (soilBlock.HasSeed())
                {
                    _countData.count -= 1;

                    soilBlock.AddNutrient();

                    Sounds.Instance.PlayUseBucketSound(transform.position);
                }
            }
        }
        
        if (_countData.count <= 0)
        {
            Destroy(gameObject);
        }
    }
}