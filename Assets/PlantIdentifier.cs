using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantIdentifier : MonoBehaviour
{
    public enum PlantType
    {
        YellowFlower,
        RedBerry,
        WaterBob,
        Weeds,
    }

    public PlantType plantType;
    public Material materialLabel;
    
    public bool CompareToPlant(PlantIdentifier otherPlant)
    {
        return plantType == otherPlant.plantType;
    }
}
