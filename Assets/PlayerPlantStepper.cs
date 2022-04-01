using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlantStepper : MonoBehaviour
{
    private List<Tuple<SeedGrowth, float>> _hurtCooldowns = new List<Tuple<SeedGrowth, float>>();

    void Update()
    {
        var groundUnderHighlight = Physics.RaycastAll(new Ray(transform.position + Vector3.up, Vector3.down), 3f)
            .Where(hit =>
            {
                if (hit.collider.isTrigger) return false;

                var seedGrowth = hit.collider.GetComponent<SeedGrowth>();
                if (!seedGrowth) return false;

                return !_hurtCooldowns.Any(pair => pair.Item1 == seedGrowth);
            }).ToArray();
        if (groundUnderHighlight.Length > 0)
        {
            var seedGrowth = groundUnderHighlight[0].collider.GetComponent<SeedGrowth>();
            seedGrowth.Hurt();

            _hurtCooldowns.Add(new Tuple<SeedGrowth, float>(seedGrowth, Time.time + 1f));
        }

        if (_hurtCooldowns.Count > 0)
        {
            var newCooldowns = new List<Tuple<SeedGrowth, float>>();
            foreach (var hurtCooldown in _hurtCooldowns)
            {
                if (Time.time < hurtCooldown.Item2)
                {
                    newCooldowns.Add(hurtCooldown);
                }
            }

            _hurtCooldowns = newCooldowns;
        }
    }
}