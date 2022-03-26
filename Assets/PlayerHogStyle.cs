using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHogStyle : MonoBehaviour
{
    private static int _index = 0;

    public Material[] styles;
    public GameObject target;

    void Start()
    {
        var style = styles[_index++];
        target.GetComponentInChildren<SkinnedMeshRenderer>().materials = new[] {style};
    }
}