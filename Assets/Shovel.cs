using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    public GameObject riverTemplate;

    private void Start()
    {
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
                var waterHits = Physics.OverlapSphere(highlightPosition, .6f).Where(hit => hit.CompareTag("River"))
                    .ToArray();
                if (waterHits.Length > 0)
                {
                    var colliderTransform = raycastHit.collider.transform;
                    var position = colliderTransform.position;
                    var parent = colliderTransform.parent;
                    var rotation = colliderTransform.rotation;
                    Destroy(raycastHit.collider.gameObject);
                    Instantiate(riverTemplate, position, rotation, parent);

                    Sounds.Instance.PlayHoeSound(transform.position);
                }
            }
        }
    }
}