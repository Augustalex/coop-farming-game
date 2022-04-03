using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Ghost;
using UnityEngine;

public class AllRoundHammer : MonoBehaviour
{
    public BuildableItem[] items;

    private BuildableItem _currentItem;
    private SmartItemGhost _ghost;
    private int _itemIndex;
    private bool _demolishMode;

    private ActionGhost _demolishActionGhost;
    private ItemPriceOverheadText _priceDisplay;

    void Start()
    {
        _ghost = GetComponent<SmartItemGhost>();

        _demolishActionGhost = GetComponent<ActionGhost>();
        _demolishActionGhost.Deactivate();

        _priceDisplay = GetComponentInChildren<ItemPriceOverheadText>();

        _currentItem = items[0];
        ApplyItem(_currentItem);

        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    private void ApplyItem(BuildableItem currentItem)
    {
        _ghost.ApplyConfiguration(currentItem.itemGhostConfiguration);
        _priceDisplay.SetText($"${currentItem.cost}");
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (_demolishMode)
        {
            if (_demolishActionGhost.IsValidLocation(highlightPosition))
            {
                DestroyNearbyFences(highlightPosition);
            }
        }
        else if (_ghost.IsValidLocation(highlightPosition))
        {
            if (_currentItem.cost <= GameManager.Instance.money)
            {
                var rawHits = Physics.RaycastAll(new Ray(highlightPosition + Vector3.up * 1f, Vector3.down), 4f);
                if (rawHits.Any(hit => hit.collider.CompareTag("Item"))) return;

                var hits = rawHits.Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
                if (hits.Length == 0) return;

                var colliderTransform = hits[0].collider.transform;

                var ghostRotationEuler = _ghost.GhostTransform().rotation.eulerAngles;
                var currentRotationEuler = transform.rotation.eulerAngles;

                var tile = Instantiate(
                    _currentItem.template,
                    colliderTransform.position + Vector3.up * .5f,
                    Quaternion.Euler(
                        currentRotationEuler.x,
                        ghostRotationEuler.y,
                        currentRotationEuler.z
                    ),
                    colliderTransform.parent
                );

                tile.GetComponent<ConstructedItem>().Construct();
                GameManager.Instance.UseMoney(_currentItem.cost);
                Sounds.Instance.PlayPlaceTileSound(highlightPosition);
                Sounds.Instance.PlayTinyBuySound(highlightPosition);
            }
        }
    }

    private void DestroyNearbyFences(Vector3 highlightPosition)
    {
        var demolishedSomething = false;

        foreach (var hit in _demolishActionGhost.HitsForPosition(highlightPosition))
        {
            var constructedItem = hit.GetComponent<ConstructedItem>();
            if (constructedItem)
            {
                demolishedSomething = true;
                constructedItem.Demolish();
            }
        }

        if (demolishedSomething)
        {
            Sounds.Instance.PlayHoeSound(transform.position);
        }
    }

    public void Cycle()
    {
        _itemIndex += 1;

        if (_itemIndex == items.Length)
        {
            Debug.Log("DEMOLISH ON!");
            _demolishMode = true;
            _ghost.Deactivate();
            _demolishActionGhost.Activate();

            _priceDisplay.SetText("");
        }
        else if (_itemIndex > items.Length)
        {
            Debug.Log("DEMOLISH OFF!");
            _demolishMode = false;
            _itemIndex = 0;
        }

        if (_itemIndex < items.Length)
        {
            Debug.Log("NEXT ITEM");
            _currentItem = items[_itemIndex];
            ApplyItem(_currentItem);

            _ghost.Activate();
            _demolishActionGhost.Deactivate();
        }
    }
}