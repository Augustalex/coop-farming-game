using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class JobGenerator : MonoBehaviour
{
    public GameObject jobTemplate;
    public GameObject leftTopPivot;
    private const float Offset = .75f;

    private float _index;

    private int _maxJobs = 3;

    void Start()
    {
        GameManager.Instance.StarLevelChanged += UpdateJobCount;

        for (int i = 0; i < _maxJobs; i++)
        {
            CreateJobPaper(i);
        }
    }

    private void CreateJobPaper(int i)
    {
        var job = Instantiate(jobTemplate,
            leftTopPivot.transform.position + Vector3.right * Offset * _index,
            leftTopPivot.transform.rotation,
            leftTopPivot.transform);
        _index += 1;

        var jobPaperRoot = job.GetComponent<JobPaperRoot>();
        if (i == 3)
        {
            jobPaperRoot.waterJob = true;
        }

        jobPaperRoot.SetupWithRandomJob();
    }

    private void UpdateJobCount(int starLevel)
    {
        if (starLevel == 3)
        {
            if (_maxJobs == 3)
            {
                var nextIndex = _maxJobs - 1;
                CreateJobPaper(nextIndex);
                _maxJobs = 4;
            }
        }
    }
}