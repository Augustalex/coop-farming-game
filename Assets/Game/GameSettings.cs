using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public float truckStartDelay = 1f;
        public float truckTimeToMove = 30f;
        public float truckTimeToMoveAway = 5f;
        public float deliveryTime = 60f;
        public float truckTimeBetweenDeliveries = 60f;
        
        public int fullWaterCharge = 3;
        public int fullTilePile = 10;
    }
}