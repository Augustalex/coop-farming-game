using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(UseOnSoil))]
[RequireComponent(typeof(CountData))]
public class SeedSack : MonoBehaviour
{
    private readonly Stack<GameObject> _seeds = new Stack<GameObject>();

    public MeshRenderer itemLabel;
    private Material _emptyMaterial;
    private CountData _countData;
    private UseOnSoil _useOnSoil;

    void Start()
    {
        _countData = GetComponent<CountData>();
        _useOnSoil = GetComponent<UseOnSoil>();
        _emptyMaterial = itemLabel.material;

        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (_seeds.Count > 0)
        {
            var soil = _useOnSoil.HoverOnSoil(highlightPosition);
            if (soil)
            {
                var soilBlock = soil.GetComponent<SoilBlock>();
                if (soilBlock.IsFree())
                {
                    var item = _seeds.Pop();
                    if (item)
                    {
                        item.SetActive(true);
                        item.GetComponent<SeedItem>().OnUseOnSoil(soilBlock);
                    }

                    UpdateCount();
                }
            }
        }
        else
        {
            Sounds.Instance.PlayFailedToUseBucketSound(transform.position);
        }
    }

    public void UpdateCount()
    {
        _countData.count = _seeds.Count;

        Sounds.Instance.PlayUseBucketSound(transform.position);

        if (_seeds.Count == 0)
        {
            itemLabel.materials = new[] {_emptyMaterial};
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
                var topSeedItem = _seeds.Peek();
                if (!topSeedItem) // if Item has been destroyed for some reason
                {
                    _seeds.Pop();
                    UpdateCount();
                }
                else if (_seeds.Count == 0 || topSeedItem.GetComponent<SeedItem>().CompareToSeedItem(seedItem))
                {
                    if (_seeds.Count == 0)
                    {
                        itemLabel.materials = new[]
                            {seedItem.GetComponentInChildren<MeshRenderer>().material};
                    }

                    // var playerItem = seedItem.GetComponent<PlayerItem>();
                    // if (playerItem.IsGrabbed())
                    // {
                    //     playerItem.Steal();
                    // }

                    var item = seedItem.gameObject;
                    item.SetActive(false);

                    _seeds.Push(item);
                    _countData.count = _seeds.Count;

                    Sounds.Instance.PlayAddedToSeedSack(transform.position);
                }
            }
        }
    }
}