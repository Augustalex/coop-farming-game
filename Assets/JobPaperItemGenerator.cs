using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class JobPaperItemGenerator : MonoBehaviour
{
    public GameObject yellowSeedTemplate;
    public GameObject redSeedTemplate;
    public GameObject waterSeedTemplate;
    public GameObject rodSeedTemplate;
    public GameObject deadlineTemplate;

    public GameObject listPivot;

    private float _offset = .125f;
    private int _index;
    private List<GameObject> _items = new List<GameObject>();
    private Job _currentJob;

    private void Update()
    {
        if (_currentJob.time > 0)
        {
            _items[_index - 1].GetComponentInChildren<TMP_Text>().text =
                $"{Mathf.Ceil((_currentJob.time - Time.time) / 60)}min";
        }   
    }

    public void Generate(Job job)
    {
        _currentJob = job;
        
        if (_items.Count > 0)
        {
            _items.ForEach(Destroy);
            _items.Clear();
            _index = 0;
        }

        if (job.yellowSeeds > 0) AddToList(job.yellowSeeds.ToString(), yellowSeedTemplate);
        if (job.redSeeds > 0) AddToList(job.redSeeds.ToString(), redSeedTemplate);
        if (job.waterSeeds > 0) AddToList(job.waterSeeds.ToString(), waterSeedTemplate);
        if (job.rodSeeds > 0) AddToList(job.rodSeeds.ToString(), rodSeedTemplate);
        if (job.time > 0) AddToList($"{Mathf.Ceil((job.time - Time.time) / 60)}min", deadlineTemplate);
    }

    public void AddToList(string text, GameObject template)
    {
        var item = Instantiate(template, listPivot.transform.position + Vector3.down * _offset * _index,
            listPivot.transform.rotation, listPivot.transform);
        item.GetComponentInChildren<TMP_Text>().text = text;
        _index += 1;

        _items.Add(item);
    }
}