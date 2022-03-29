using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullsDisplay : MonoBehaviour
{
    public GameObject skullTemplate;

    private const float OffsetX = .18f;

    void Start()
    {
        GameManager.Instance.FailAttemptsChanged += UpdateSkullCount;
        UpdateSkullCount(0);
    }

    private void UpdateSkullCount(int skullCount)
    {
        foreach (var componentsInChild in GetComponentsInChildren<MeshRenderer>())
        {
            Destroy(componentsInChild.gameObject, .1f);
        }

        for (int i = 0; i < skullCount; i++)
        {
            Instantiate(skullTemplate, transform.position + Vector3.right * OffsetX * i, Quaternion.identity,
                transform);
        }
    }
}