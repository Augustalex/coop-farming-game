using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(CountData))]
public class BasketItem : MonoBehaviour
{
    public MeshRenderer itemLabel;
    private Material _emptyMaterial;
    private readonly Stack<GameObject> _plants = new Stack<GameObject>();
    private List<Tuple<GameObject, float>> _droppedRecently = new List<Tuple<GameObject, float>>();
    private Vector3 _originalScale;
    private CountData _countData;

    public bool carryWaterItems = false;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
        _countData = GetComponent<CountData>();

        _emptyMaterial = itemLabel.material;

        _originalScale = transform.localScale;
    }

    private void Update()
    {
        if (_droppedRecently.Count > 0)
        {
            var removeSomething = false;
            foreach (var tuple in _droppedRecently)
            {
                if (Time.time > tuple.Item2) removeSomething = true;
            }

            if (removeSomething)
            {
                _droppedRecently = _droppedRecently.Where(itemTimePair => Time.time < itemTimePair.Item2).ToList();
            }
        }

        transform.localScale = _originalScale + Vector3.one * (((float) _plants.Count / (float) _countData.max) * .8f);
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (_plants.Count > 0)
        {
            var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
                .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
            if (hits.Length > 0)
            {
                var hit = hits[0];
                if (hit.collider.CompareTag("Grass"))
                {
                    var item = _plants.Pop();
                    _countData.count = _plants.Count;
                    if (_plants.Count == 0)
                    {
                        itemLabel.materials = new[] {_emptyMaterial};
                    }

                    Sounds.Instance.PlayUseBucketSound(transform.position);

                    if (item)
                    {
                        item.SetActive(true);
                        item.transform.position = hit.transform.position + Vector3.up * 1f;


                        if (_plants.Count == 0)
                        {
                            itemLabel.materials = new[] {_emptyMaterial};
                        }

                        _droppedRecently.Add(new Tuple<GameObject, float>(item, Time.time + 5f));
                    }
                    else
                    {
                        Debug.Log("Item from bucket was already destroyed when dropped");
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
        if (_plants.Count >= _countData.max) return;
        if (_plants.Contains(other.gameObject) || _droppedRecently.Any(pair => pair.Item1 == other.gameObject)) return;

        if (carryWaterItems)
        {
            if (other.CompareTag("Item"))
            {
                var waterSeedItem = other.GetComponent<WaterSeedItem>();
                if (waterSeedItem)
                {
                    if (_plants.Any(plant => plant.GetComponent<Plant>())) return;
                    if (_plants.Count == 0 ||
                        _plants.Peek().GetComponent<WaterSeedItem>().CompareToSeedItem(waterSeedItem))
                    {
                        if (_plants.Count == 0)
                        {
                            itemLabel.materials = new[]
                                {waterSeedItem.materialLabel};
                        }

                        // var playerItem = waterSeedItem.GetComponent<PlayerItem>();
                        // if (playerItem.IsGrabbed())
                        // {
                        //     playerItem.Steal();
                        // }

                        var item = waterSeedItem.gameObject;
                        item.SetActive(false);

                        _plants.Push(item);
                        _countData.count = _plants.Count;

                        Sounds.Instance.PlayAddedToSeedSack(transform.position);
                    }
                }
            }
        }
        else
        {
            if (other.CompareTag("Goods"))
            {
                var plantItem = other.GetComponent<Plant>();
                if (plantItem)
                {
                    if (_plants.Count == 0 || _plants.Peek().GetComponent<Plant>().CompareToPlant(plantItem))
                    {
                        if (_plants.Count == 0)
                        {
                            itemLabel.materials = new[]
                                {plantItem.GetPlantIdentifier().materialLabel};
                        }

                        // var playerItem = plantItem.GetComponent<PlayerItem>();
                        // if (playerItem.IsGrabbed())
                        // {
                        //     playerItem.Steal();
                        // }

                        if (plantItem.InSoil())
                        {
                            plantItem.Harvest();
                        }

                        var item = plantItem.gameObject;
                        item.SetActive(false);

                        _plants.Push(item);
                        _countData.count = _plants.Count;

                        Sounds.Instance.PlayAddedToSeedSack(transform.position);
                    }
                }
            }
        }
    }
}