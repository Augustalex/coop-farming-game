using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ConstructedBrigde : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<ConstructedItem>().Constructed += OnConstructed;
        }

        private void OnConstructed()
        {
            var bridge = GetComponent<BrigdeIdentifier>();

            var rivers = Physics.RaycastAll(new Ray(transform.position + Vector3.up * 1f, Vector3.down), 4f)
                .Where(hit => hit.collider.CompareTag("River")).ToArray();
            if (rivers.Length == 0) throw new Exception("Tried building bridge - but was not built on a river.");

            var river = rivers[0];
            bridge.SetRiver(river.collider.GetComponent<RiverSoil>());
        }
    }
}