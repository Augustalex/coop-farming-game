using System;
using UnityEngine;

public class ItemGhost : MonoBehaviour
{
    public GameObject ghostTemplate;
    private GameObject _ghost;

    void Start()
    {
        _ghost = Instantiate(ghostTemplate);
    }

    public void Move(Vector3 position)
    {
        _ghost.transform.position = position;
    }

    public void Rotate()
    {
        _ghost.transform.RotateAround(transform.position, Vector3.up, 90f);
    }

    public Transform GhostTransform()
    {
        return _ghost.transform;
    }
}