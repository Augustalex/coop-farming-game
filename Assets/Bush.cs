using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bush : MonoBehaviour
{
    public GameObject seedTemplate;
    
    public GameObject[] levels;
    
    public int levelIndex;
    
    private float _refreshCooldown;

    private void Update()
    {
        if (_refreshCooldown <= 0)
        {
            _refreshCooldown = 120f;
            levelIndex = Mathf.Max(0, levelIndex - 1);
            ReloadLevels();
        }
        else
        {
            _refreshCooldown -= Time.deltaTime;
        }
    }

    public bool CanHarvest()
    {
        return levelIndex < levels.Length;
    }

    public void Harvest()
    {
        if (!CanHarvest()) return;

        levelIndex = Mathf.Min(levels.Length, levelIndex + 1);
        SpawnSeeds();

        ReloadLevels();
    }

    private void ReloadLevels()
    {
        for (var i = 0; i < levels.Length; i++)
        {
            if (i >= levelIndex)
            {
                Visible(levels[i]);
            }
            else
            {
                Hidden(levels[i]);
            }
        }
    }

    private void Visible(GameObject level)
    {
        level.SetActive(true);
    }

    private void Hidden(GameObject level)
    {
        level.SetActive(false);
    }

    private void SpawnSeeds()
    {
        var routineSeedTemplate = seedTemplate;
        var originPosition = transform.position;

        var seeds = Instantiate(routineSeedTemplate);
        seeds.transform.position = originPosition + Vector3.up * 1.5f;
        var body = seeds.GetComponent<Rigidbody>();
        body.AddForce(Vector3.up + Random.insideUnitSphere * 1.5f, ForceMode.Impulse);
    }
}