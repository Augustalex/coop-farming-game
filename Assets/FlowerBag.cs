using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class FlowerBag : MonoBehaviour
{
    public GameObject flowerTemplate;

    private CountData _countData;

    void Start()
    {
        _countData = GetComponent<CountData>();

        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
            .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var raycastHit = hits[0];
            if (raycastHit.collider.CompareTag("Grass"))
            {
                var grass = raycastHit.collider.GetComponent<GrassBlock>();
                if (!grass.HasPlant())
                {
                    grass.Plant(flowerTemplate);
                    Sounds.Instance.PlayHoeSound(transform.position);
                    _countData.count -= 1;
                }
            }
        }

        if (_countData.count == 0)
        {
            Destroy(gameObject);
        }
    }
}