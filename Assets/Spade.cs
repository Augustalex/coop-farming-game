using System.Linq;
using Game;
using UnityEngine;

public class Spade : MonoBehaviour
{
    private void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    void OnUse(Vector3 highlightPosition)
    {
        var rawHits = Physics.OverlapSphere(highlightPosition, .3f);
        var hits = rawHits.Where(hit => hit.GetComponent<SeedSource>()).ToArray();
        if (hits.Length > 0)
        {
            var seedSourceObject = hits[0];
            var seedSource = seedSourceObject.GetComponent<SeedSource>();
            seedSource.Pick();

            Sounds.Instance.PlayPickBushSound(highlightPosition);
        }
        else
        {
            var rayHits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
                .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
            if (rayHits.Length > 0)
            {
                var raycastHit = rayHits[0];
                if (raycastHit.collider.CompareTag("Grass"))
                {
                    var grass = raycastHit.collider.GetComponent<GrassBlock>();
                    if (grass.HasPlant())
                    {
                        grass.RemovePlant();
                        Sounds.Instance.PlayRemoveFlowerSound(transform.position);
                    }
                }
            }
        }
    }
}
