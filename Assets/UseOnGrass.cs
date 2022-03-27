using System.Linq;
using Game;
using UnityEngine;

public class UseOnGrass : MonoBehaviour
{
    public GameObject HoveringGrass(Vector3 highlightPosition)
    {     
        var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
            .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var hit = hits[0];
            if (hit.collider.CompareTag("Grass"))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }
}