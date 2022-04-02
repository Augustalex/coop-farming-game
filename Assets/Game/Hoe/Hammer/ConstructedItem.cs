using System;
using UnityEngine;

namespace Game
{
    public class ConstructedItem : MonoBehaviour
    {
        public event Action Constructed;
        public event Action Demolished;

        public void Construct()
        {
            Constructed?.Invoke();
        }

        public void Demolish()
        {
            Demolished?.Invoke();
            Destroy(gameObject);
        }
    }
}