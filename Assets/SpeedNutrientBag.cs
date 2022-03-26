using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class SpeedNutrientBag : MonoBehaviour
{
    private int _uses = 10;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (!gameObject) return;
        if (_uses <= 0)
        {
            Destroy(gameObject);
            return;
        }

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
                    _uses -= 1;

                    soilBlock.AddSpeedNutrient();

                    Sounds.Instance.PlayUseBucketSound(transform.position);
                }
            }
        }
    }
}
