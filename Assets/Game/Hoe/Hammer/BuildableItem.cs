using Game.Ghost;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BuildableItem", menuName = "Buildable Item", order = 0)]
    public class BuildableItem : ScriptableObject
    {
        public GameObject template;
        public ItemGhostConfiguration itemGhostConfiguration;
        public int cost;
    }
}