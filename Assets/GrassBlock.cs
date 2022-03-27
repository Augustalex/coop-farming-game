using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : MonoBehaviour
{
    private GameObject _plant;

    public void Plant(GameObject template)
    {
        _plant = Instantiate(template, transform.position + Vector3.up * .5f, transform.rotation, transform);

        var flowerGrowth = _plant.GetComponent<FlowerGrowth>();
        if (flowerGrowth)
        {
            flowerGrowth.Died += () => _plant = null;
        }
    }

    public bool HasPlant()
    {
        return _plant != null;
    }

    public void RemovePlant()
    {
        Destroy(_plant);
        _plant = null;
    }

    public GameObject GetPlant()
    {
        return _plant;
    }
}