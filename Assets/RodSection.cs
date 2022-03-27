using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodSection : MonoBehaviour
{
    public GameObject rodSeedItem;
    public GameObject[] rodTemplates;

    private List<GameObject> _rods = new List<GameObject>();

    void Start()
    {
        foreach (var child in GetComponentsInChildren<RodSingle>())
        {
            Destroy(child.gameObject);
        }

        var start = transform.position + new Vector3(-.25f, 0, -.25f);
        var current = start;

        var increment = .25f;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                var randomRod = rodTemplates[Random.Range(0, rodTemplates.Length)];
                var rod = Instantiate(randomRod, current, Quaternion.identity, transform);
                _rods.Add(rod);

                current += Vector3.forward * increment;
            }

            current.z = start.z;
            current += Vector3.right * increment;
        }
    }

    public void Pick()
    {
        foreach (var rod in _rods)
        {
            Destroy(rod);
            Instantiate(rodSeedItem, rod.transform.position + Vector3.up * Random.Range(1, 3), Random.rotation, null);
        }

        Destroy(gameObject);
    }
}