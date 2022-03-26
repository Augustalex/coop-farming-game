using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private PlayerLooker _playerBoody;

    private void Awake()
    {
        _playerBoody = transform.parent.GetComponentInChildren<PlayerLooker>();
    }

    private void Update()
    {
        var hits = Physics.RaycastAll(new Ray(transform.position, Vector3.down), 3f)
            .Where(hit => hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var ground = hits[0].collider.transform.position;
            var currentPosition = transform.position;
            transform.position = new Vector3(
                currentPosition.x,
                ground.y + .7f,
                currentPosition.z);
        }
    }

    public void MoveAlong(Vector3 move)
    {
        var target = _playerBoody.transform.position + move.normalized * 1f;
        transform.position = AlignToGrid(target);
    }

    private Vector3 AlignToGrid(Vector3 point)
    {
        return new Vector3(
            Mathf.Round(point.x),
            Mathf.Round(point.y),
            Mathf.Round(point.z)
        );
    }
}