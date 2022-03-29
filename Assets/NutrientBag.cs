using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(UseOnSoil))]
[RequireComponent(typeof(CountData))]
public class NutrientBag : MonoBehaviour
{
    private CountData _countData;
    private UseOnSoil _useOnSoil;

    void Start()
    {
        _countData = GetComponent<CountData>();
        _useOnSoil = GetComponent<UseOnSoil>();

        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (!gameObject) return;

        var soil = _useOnSoil.HoverOnSoil(highlightPosition);
        if (soil)
        {
            var soilBlock = soil.GetComponent<SoilBlock>();
            if (soilBlock.HasSeed())
            {
                _countData.count -= 1;

                soilBlock.AddNutrient();

                Sounds.Instance.PlayUseBucketSound(transform.position);
            }
        }

        if (_countData.count <= 0)
        {
            Destroy(gameObject);
        }
    }
}