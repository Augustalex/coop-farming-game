using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boping : MonoBehaviour
{
    public float frequency = 1.8f;
    public float height = .035f;

    private float _direction = 1f;
    private float _progress = 0f;
    private Vector3 _originalPosition;

    void Start()
    {
        _originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        _progress += (Time.deltaTime / frequency) * _direction;
        if (_progress > 1 || _progress < 0)
        {
            _direction *= -1;
        }

        var currentPosition = transform.localPosition;
        var newHeight = Vector3.Slerp(_originalPosition - Vector3.up * height,
            _originalPosition + Vector3.up * height, _progress).y;
        transform.localPosition = new Vector3(
            currentPosition.x,
            newHeight,
            currentPosition.z
        );
    }
}