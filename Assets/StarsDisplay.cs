using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsDisplay : MonoBehaviour
{
    public GameObject fullStar;
    public GameObject emptyStar;

    private const float OffsetX = .18f;

    void Start()
    {
        UpdateStarCount(0);

        GameManager.Instance.StarLevelChanged += UpdateStarCount;
    }

    private void UpdateStarCount(int starCount)
    {
        foreach (var componentsInChild in GetComponentsInChildren<MeshRenderer>())
        {
            Destroy(componentsInChild.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            var template = i < starCount ? fullStar : emptyStar;
            Instantiate(template, transform.position + Vector3.right * OffsetX * i, Quaternion.identity, transform);
        }

        if (starCount == 4)
        {
            var template = fullStar;
            var index = starCount - 1;
            Instantiate(template, transform.position + Vector3.right * OffsetX * index, Quaternion.identity, transform);
        }
    }
}