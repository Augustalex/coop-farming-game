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

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            var job = Instantiate(jobTemplate,
                leftTopPivot.transform.position + Vector3.right * Offset * _index,
                leftTopPivot.transform.rotation,
                leftTopPivot.transform);
            _index += 1;
            
            job.GetComponent<JobPaperRoot>().SetupWithRandomJob();
        }
    }
}