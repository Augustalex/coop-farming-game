using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

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
            var soil = _useOnSoil.HoveringSoil(highlightPosition);
            if (soil)
            {
                var soilBlock = soil.GetComponent<SoilBlock>();
                if (soilBlock.IsFree())
                {
                    var item = _seeds.Pop();
                    _countData.count = _seeds.Count;

                    item.SetActive(true);

                    item.GetComponent<SeedItem>().OnUseOnSoil(soilBlock);

                    Sounds.Instance.PlayUseBucketSound(transform.position);

                    if (_seeds.Count == 0)
                    {
                        itemLabel.materials = new[] {_emptyMaterial};
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
                    if (playerItem.IsGrabbed())
                    {
                        playerItem.Steal();
                    }

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