using System;
using UnityEngine;

public class ItemGhost : MonoBehaviour
{
    public GameObject ghostTemplate;
    private GameObject _ghost;
    private PlayerItem _playerItem;

    void Start()
    {
        _ghost = Instantiate(ghostTemplate);

        _playerItem = GetComponent<PlayerItem>();

        _playerItem.Dropped += OnDrop;
        _playerItem.Escaped += OnDrop;

        _playerItem.Grabbed += OnGrabbed;
    }

    private void OnGrabbed()
    {
        _ghost.SetActive(true);
    }

    private void OnDrop()
    {
        _ghost.SetActive(false);
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