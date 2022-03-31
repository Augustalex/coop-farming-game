using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyesController : MonoBehaviour
{
    private float _blinkCooldown;
    private float _blinkStarted;
    private GameObject[] _pupils;
    private float _moveCooldown;
    private Vector2 _moveTo;
    private float _moveToProgress;
    private Vector3 _moveStart;
    private float _stayCooldown;
    private float _openStarted;

    private const float PupilSpeed = 4f;
    private const float BlinkSpeed = 6f;

    void Start()
    {
        _pupils = GetComponentsInChildren<EyePupil>().Select(e => e.gameObject).ToArray();
    }

    void Update()
    {
        if (_blinkStarted > 0)
        {
            var progress = 1f - _blinkStarted;
            transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), progress);

            _blinkStarted -= Time.deltaTime * BlinkSpeed;

            if (_blinkStarted <= 0)
            {
                _stayCooldown = Random.Range(.5f, 1f);
            }
        }
        else if (_stayCooldown > 0)
        {
            _stayCooldown -= Time.deltaTime;

            if (_stayCooldown <= 0)
            {
                _openStarted = 1f;
            }
        }
        else if (_openStarted > 0)
        {
            _openStarted -= Time.deltaTime * BlinkSpeed;

            transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), 1f - _openStarted);
        }
        else if (_blinkCooldown > 0)
        {
            _blinkCooldown -= Time.deltaTime;

            UpdatePupils();
        }
        else
        {
            Blink();
        }
    }

    private void UpdatePupils()
    {
        if (_moveToProgress > 0)
        {
            var progress = 1f - _moveToProgress;

            var target = new Vector3(
                _moveTo.x,
                _moveStart.y,
                _moveTo.y
            );

            foreach (var pupil in _pupils)
            {
                pupil.transform.localPosition = Vector3.Lerp(_moveStart, target, progress);
            }

            _moveToProgress -= Time.deltaTime * PupilSpeed;
        }
        else if (_moveCooldown > 0)
        {
            _moveCooldown -= Time.deltaTime;
        }
        else
        {
            MovePupils();
        }
    }

    private void MovePupils()
    {
        _moveCooldown = Random.Range(.8f, 3f);

        _moveStart = _pupils[0].transform.localPosition;
        _moveTo = Random.insideUnitCircle.normalized * .5f;
        _moveToProgress = 1f;
    }

    public void Blink()
    {
        _blinkCooldown = Random.Range(2, 4);

        _blinkStarted = 1f;
    }
}