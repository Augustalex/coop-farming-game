using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DashScare : MonoBehaviour
{
    private GameObject _body;

    void Start()
    {
        _body = GetComponentInChildren<PlayerLooker>().gameObject;
        GetComponent<PlayerController>().Dashed += OnDash;
    }

    private void OnDash()
    {
        var cubes = Physics.OverlapSphere(_body.transform.position, 4f).Where(hit => hit.GetComponent<WildCube>());
        foreach (var cube in cubes)
        {
            var plant = cube.GetComponent<Plant>();
            if (plant && plant.InSoil()) continue;

            var playerItem = cube.GetComponent<PlayerItem>();
            if (playerItem && playerItem.IsGrabbed()) continue;

            var wildCube = cube.GetComponent<WildCube>();
            wildCube.RunAwayFromDash(_body.transform.position);
        }
    }
}