using System.Linq;
using Game;
using UnityEngine;

public class Spade : MonoBehaviour
{
    private void Start()
    {
        GetComponent<PlayerItem>().UseItem += OnUse;
        GetComponent<PlayerItem>().UseItem += OnUse;
    }

    void OnUse(Vector3 highlightPosition)
    {
        var rawHits = Physics.OverlapBox(highlightPosition, PlayerGrabber.SelectColumn);
        var seedSources = rawHits.Where(hit => hit.GetComponent<SeedSource>()).ToArray();
        if (seedSources.Length > 0)
        {
            var seedSourceObject = seedSources[0];
            var seedSource = seedSourceObject.GetComponent<SeedSource>();
            seedSource.Pick();

            Sounds.Instance.PlayPickBushSound(highlightPosition);
        }
        else
        {
            var soilBlocks = rawHits.Where(hit => hit.GetComponent<SoilBlock>()).ToArray();
            if (soilBlocks.Length > 0)
            {
                var soilBLockHit = soilBlocks[0];
                var soilBlock = soilBLockHit.GetComponent<SoilBlock>();
                soilBlock.KillWeeds();

                Sounds.Instance.PlayPickBushSound(highlightPosition);
            }
            else
            {
                var rayHits = Physics.OverlapBox(highlightPosition, PlayerGrabber.SelectColumn)
                    .Where(hit => hit.GetComponent<Interactable>()).ToArray();
                if (rayHits.Length > 0)
                {
                    var raycastHit = rayHits[0];
                    if (raycastHit.CompareTag("Grass"))
                    {
                        var grass = raycastHit.GetComponent<GrassBlock>();
                        if (grass && grass.HasPlant())
                        {
                            grass.RemovePlant();
                            Sounds.Instance.PlayRemoveFlowerSound(transform.position);
                        }
                    }
                }
            }
        }
    }
}