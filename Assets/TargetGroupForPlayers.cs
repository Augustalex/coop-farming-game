using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game;
using UnityEngine;

public class TargetGroupForPlayers : MonoSingleton<TargetGroupForPlayers>
{
    private List<Transform> _targetGroup = new List<Transform>();
    private CinemachineTargetGroup _targetMachine;

    void Start()
    {
        _targetMachine = GetComponent<CinemachineTargetGroup>();
    }
    private void LateUpdate()
    {
        var target = Vector3.zero;
        foreach (var transform1 in _targetGroup)
        {
            target += transform1.position;
        }

        if (target != Vector3.zero)
        {
            target /= _targetGroup.Count;

            transform.position = target;
        }
    }

    public void AddPlayer(GameObject player)
    {
        // _targetGroup.Add(player.transform);
        _targetMachine.AddMember(player.transform, 1f, 10f);
    }
}
