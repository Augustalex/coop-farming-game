using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Hoe : MonoBehaviour
    {
        public GameObject soilTemplate;

        private void Start()
        {
            GetComponent<PlayerItem>().UseItem += OnUse;

            soilTemplate = AssetLibrary.Instance.soilTemplate;
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
                    var colliderTransform = raycastHit.collider.transform;
                    var position = colliderTransform.position;
                    var parent = colliderTransform.parent;
                    var rotation = colliderTransform.rotation;
                    Destroy(raycastHit.collider.gameObject);
                    Instantiate(soilTemplate, position, rotation, parent);

                    Sounds.Instance.PlayHoeSound(transform.position);
                }
            }
        }
    }
}