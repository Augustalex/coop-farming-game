using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boping : MonoBehaviour
{
    private float _direction = 1f;
    private float _progress = 0f;
    private float _frequency = 1.8f;
    private float _height = .035f;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _progress += (Time.deltaTime / _frequency) * _direction;
        if (_progress > 1 || _progress < 0)
        {
            _direction *= -1;
        }

        transform.position = Vector3.Lerp(_originalPosition - Vector3.up * _height,
            _originalPosition + Vector3.up * _height, _progress);
    }
}