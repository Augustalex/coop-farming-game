using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class SeedSack : MonoBehaviour
{
    private readonly Stack<GameObject> _seeds = new Stack<GameObject>();

    public MeshRenderer itemLabel;
    private Material _emptyMaterial;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;

        _emptyMaterial = itemLabel.material;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (_seeds.Count > 0)
        {
            var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
                .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
            if (hits.Length > 0)
            {
                var hit = hits[0];
                if (hit.collider.CompareTag("Soil"))
                {
                    var soil = hit.collider.gameObject;
                    var soilBlock = soil.GetComponent<SoilBlock>();
                    Debug.Log($"({Time.time}) Seed?");
                    if (soilBlock.IsFree())
                    {
                        var item = _seeds.Pop();
                        item.SetActive(true);
                        
                        Debug.Log($"({Time.time}) Seed YES!");
                        
                        item.GetComponent<SeedItem>().OnUseOnSoil(soilBlock);

                        Sounds.Instance.PlayUseBucketSound(transform.position);

                        if (_seeds.Count == 0)
                        {
                            itemLabel.materials = new[] {_emptyMaterial};
                        }
                    }
                }
            }
        }
        else
        {
            Sounds.Instance.PlayFailedToUseBucketSound(transform.position);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_seeds.Contains(other.gameObject)) return;

        if (other.CompareTag("Item"))
        {
            var seedItem = other.GetComponent<SeedItem>();
            if (seedItem)
            {
                if (_seeds.Count == 0 || _seeds.Peek().GetComponent<SeedItem>().CompareToSeedItem(seedItem))
                {
                    if (_seeds.Count == 0)
                    {
                        itemLabel.materials = new[]
                            {seedItem.GetComponentInChildren<MeshRenderer>().material};
                    }

                    var playerItem = seedItem.GetComponent<PlayerItem>();
                    if (playerItem.Grabbed())
                    {
                        playerItem.Steal();
                    }

                    var item = seedItem.gameObject;
                    item.SetActive(false);
                    _seeds.Push(item);

                    Sounds.Instance.PlayAddedToSeedSack(transform.position);
                }
            }
        }
    }
}