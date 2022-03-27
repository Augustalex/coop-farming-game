using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

[RequireComponent(typeof(ItemGhost))]
public class BridgePlacer : MonoBehaviour
{
    public GameObject bridgeTemplate;
    private ItemGhost _itemGhost;

    void Start()
    {
        _itemGhost = GetComponent<ItemGhost>();

        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    public void OnUse(Vector3 highlightPosition)
    {
        // TODO: Make so that you can build a bridge!
        // TODO: Add new "RiverSoil" component - that does not have to deal with watering of plants
        // TODO: Add new "RodPlant"
        // TODO: Add shovel to the store
        // TODO: Add plant in the river in the forrest
        // TODO: Add so that you can punch players into the ground
        // TODO: Add so you can pick players up

        var rawHits = Physics.RaycastAll(new Ray(highlightPosition + Vector3.up * 1f, Vector3.down), 4f);
        if (!rawHits.Any(hit => hit.collider.CompareTag("Item")))
        {
            var hits = rawHits.Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();

            if (hits.Length > 0)
            {
                var raycastHit = hits[0];
                if (raycastHit.collider.CompareTag("River"))
                {
                    var colliderTransform = raycastHit.collider.transform;
                    var position = colliderTransform.position;
                    var parent = colliderTransform.parent;

                    var ghostRotationEuler = _itemGhost.GhostTransform().rotation.eulerAngles;
                    var currentRotationEuler = transform.rotation.eulerAngles;
                    var rotation = Quaternion.Euler(currentRotationEuler.x, ghostRotationEuler.y,
                        currentRotationEuler.z);

                    var tile = Instantiate(bridgeTemplate);

                    tile.transform.SetParent(parent);
                    tile.transform.position = position + Vector3.up * .5f;
                    tile.transform.rotation = rotation;
                }
            }
        }
    }
}