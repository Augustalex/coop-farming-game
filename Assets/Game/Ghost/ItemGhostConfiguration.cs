using System.Collections.Generic;
using UnityEngine;

namespace Game.Ghost
{
    [CreateAssetMenu(fileName = "ItemGhostConfiguration", menuName = "Item Ghost Configuration", order = 0)]
    public class ItemGhostConfiguration : ScriptableObject
    {
        public GameObject template;
        public SmartItemGhost.GhostToggles[] toggles;

        public SmartItemGhost.GrassConstraints[] grassConstraints;

        public SmartItemGhost.SoilConstraints[] soilConstraints;
    }
}