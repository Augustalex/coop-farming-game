using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(CountData))]
public class RockPile : MonoBehaviour
{
    private GameObject _rockTileTemplate;

    private readonly Stack<GameObject> _tiles = new Stack<GameObject>();

    public bool removeWhenDepleted = false;
    public bool forShow = false;
    private CountData _countData;

    void Start()
    {
        _countData = GetComponent<CountData>();
        _rockTileTemplate = AssetLibrary.Instance.rockTile;
        
        for (int i = 0; i < _countData.count; i++)
        {
            var tile = Instantiate(_rockTileTemplate, transform.position + Vector3.up * .05f * (i + 1),
                transform.rotation, transform);
            
            _tiles.Push(tile);
        }

        if (!forShow)
        {
            GetComponent<PlayerItem>().UseItem += OnUse;
        }
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (forShow) return;
        
        if (_tiles.Count > 0)
        {
            // TODO: Make so that you can build a bridge!
            // TODO: Add new "RiverSoil" component - that does not have to deal with watering of plants
            // TODO: Add new "RodPlant"
            // TODO: Add shovel to the store
            // TODO: Add plant in the river in the forrest
            // TODO: Add so that you can punch players into the ground
            // TODO: Add so you can pick players up
            
            var rawHits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 4f);
            if (!rawHits.Any(hit => hit.collider.CompareTag("Item")))
            {
                var hits = rawHits.Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
                if (hits.Length > 0)
                {
                    var raycastHit = hits[0];
                    if (raycastHit.collider.CompareTag("Grass"))
                    {
                        var colliderTransform = raycastHit.collider.transform;
                        var position = colliderTransform.position;
                        var parent = colliderTransform.parent;
                        var rotation = colliderTransform.rotation;

                        var tile = _tiles.Pop();
                        _countData.count = _tiles.Count;
                        
                        tile.transform.SetParent(parent);
                        tile.transform.position = position + Vector3.up * .5f;
                        tile.transform.rotation = rotation;

                        if (_tiles.Count == 0)
                        {
                            Sounds.Instance.PlayPlaceTileSound(transform.position);
                            Destroy(gameObject, .1f);
                        }
                        else
                        {
                            Sounds.Instance.PlayTilePileDoneSound(transform.position);
                        }
                    }
                }
            }
        }
        else
        {
            Sounds.Instance.PlayFailedToPlaceTileSound(transform.position);
        }
    }

    public void Replenish()
    {
        if (forShow) return;
    
        for (int i = _tiles.Count; i < GameManager.Instance.gameSettings.fullTilePile; i++)
        {
            var tile = Instantiate(_rockTileTemplate, transform.position + Vector3.up * .05f * (i + 1),
                Quaternion.identity, transform);
            
            _tiles.Push(tile);
            _countData.count = _tiles.Count;
        }
    }
}