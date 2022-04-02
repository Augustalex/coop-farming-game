using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ConstructedItem))]
    public class ConstructOnGrass : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<ConstructedItem>().Constructed += OnConstructed;
        }

        private void OnConstructed()
        {
            var hits = Physics.RaycastAll(new Ray(transform.position + Vector3.up, Vector3.down), 3f)
                .Where(hit => hit.collider.CompareTag("Grass")).ToArray();
            if (hits.Length > 0)
            {
                var raycastHit = hits[0];
                raycastHit.collider.GetComponent<GrassBlock>().PlaceItem(gameObject);
            }
        }
    }
}