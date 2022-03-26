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
        var hits = Physics.OverlapSphere(highlightPosition, .3f).Where(hit => hit.GetComponent<SeedSource>()).ToArray();
        if (hits.Length > 0)
        {
            var seedSourceObject = hits[0];
            var seedSource = seedSourceObject.GetComponent<SeedSource>();
            seedSource.Pick();

            Sounds.Instance.PlayPickBushSound(highlightPosition);
        }
    }
}
