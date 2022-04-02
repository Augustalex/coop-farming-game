using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class BillboardJobPicker : MonoBehaviour, IUIController
{
    private List<UIMenuItem> _papers;
    private int _index;

    public GameObject cursor;
    private float _moveFreeze;

    public event Action Submitted;
    public event Action Exited;

    private void Awake()
    {
        DeactivateUI();
    }

    void Update()
    {
        if (cursor.activeSelf)
        {
            cursor.transform.position = _papers[_index].transform.position;
        }
    }

    public void ActivateUI()
    {
        ReloadPapers();
        cursor.SetActive(true);
    }

    public void DeactivateUI()
    {
        cursor.SetActive(false);
    }

    public void Move(Vector3 move)
    {
        if (Time.time < _moveFreeze) return;

        var startIndex = _index;

        if (move.x > 0) _index += 1;
        if (move.x < 0) _index -= 1;

        if (_index >= _papers.Count) _index = startIndex;
        if (_index < 0) _index = startIndex;

        if (_index != startIndex)
        {
            _moveFreeze = Time.time + .4f;
        }
    }

    public void PlayerSubmit()
    {
        var paper = _papers[_index];
        _index = 0;

        var jobPaper = paper.GetComponent<JobPaperRoot>();
        if (jobPaper)
        {
            if (!DeliveryManager.Instance.hasDelivery)
            {
                var job = jobPaper.GetJob();
                jobPaper.SetupWithRandomJob();
                DeliveryManager.Instance.StartDelivery(DeliveryManager.FromJob(job));
            }
        }
        else
        {
            var exitSign = paper.GetComponent<ExitSign>();
            if (exitSign)
            {
                // Exited?.Invoke(); // TODO Not needed?
            }
        }

        Submitted?.Invoke();

        Sounds.Instance.PlayPickJobSound(GameManager.Instance.GetCameraPosition());
    }

    private void ReloadPapers()
    {
        _index = 0;
        _papers = GetComponentsInChildren<UIMenuItem>().ToList();
    }

    public void MoveReset()
    {
        if (_moveFreeze > Time.time + .1f) _moveFreeze = Time.time + .1f;
    }
}