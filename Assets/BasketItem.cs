using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class BasketItem : MonoBehaviour
{
    private const int MaxSize = 10;
    public MeshRenderer itemLabel;
    private Material _emptyMaterial;
    private readonly Stack<GameObject> _plants = new Stack<GameObject>();
    private List<Tuple<GameObject, float>> _droppedRecently = new List<Tuple<GameObject, float>>();
    private Vector3 _originalScale;

    void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;

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

        transform.localScale = _originalScale + Vector3.one * _plants.Count * .08f;
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
                    item.SetActive(true);
                    item.transform.position = hit.transform.position + Vector3.up * 1f;

                    Sounds.Instance.PlayUseBucketSound(transform.position);

                    if (_plants.Count == 0)
                    {
                        itemLabel.materials = new[] {_emptyMaterial};
                    }

                    _droppedRecently.Add(new Tuple<GameObject, float>(item, Time.time + 5f));
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
        if (_plants.Count >= MaxSize) return;
        if (_plants.Contains(other.gameObject) || _droppedRecently.Any(pair => pair.Item1 == other.gameObject)) return;

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

                    var playerItem = plantItem.GetComponent<PlayerItem>();
                    if (playerItem.Grabbed())
                    {
                        playerItem.Steal();
                    }

                    if (plantItem.InSoil())
                    {
                        plantItem.Harvest();
                    }

                    var item = plantItem.gameObject;
                    item.SetActive(false);
                    _plants.Push(item);

                    Sounds.Instance.PlayAddedToSeedSack(transform.position);
                }
            }
        }
    }
}