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
    public GameObject tileTemplate;

    private readonly Stack<GameObject> _tiles = new Stack<GameObject>();

    public bool forShow = false;
    private CountData _countData;

    void Start()
    {
        _countData = GetComponent<CountData>();

        for (int i = 0; i < _countData.count; i++)
        {
            var tile = Instantiate(tileTemplate, transform.position + Vector3.up * .05f * (i + 1),
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

                        if (tile) // In case it was removed by some other code, it's just a game object in the wild after all
                        {
                            tile.transform.SetParent(parent);
                            tile.transform.position = position + Vector3.up * .5f;
                            tile.transform.rotation = rotation;
                        }

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
            var tile = Instantiate(tileTemplate, transform.position + Vector3.up * .05f * (i + 1),
                Quaternion.identity, transform);

            _tiles.Push(tile);
            _countData.count = _tiles.Count;
        }
    }
}