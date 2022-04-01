using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WildCube : MonoBehaviour
{
    private Rigidbody _body;
    private float _jumpCooldown;
    private Plant _plant;

    public event Action Jumped;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _plant = GetComponent<Plant>();
    }

    void Update()
    {
        if (_plant && _plant.InSoil()) return;

        if (_jumpCooldown < 0)
        {
            var randomDirection = Random.insideUnitCircle;
            var randomFlatDirection = new Vector3(
                randomDirection.x,
                0,
                randomDirection.y
            );
            _body.AddForce(randomFlatDirection * 3f + Vector3.up * 4f, ForceMode.Impulse);
            
            Jumped?.Invoke();
            _jumpCooldown = Random.Range(4, 7);
        }
        else
        {
            _jumpCooldown -= Time.deltaTime;
        }
    }
}