using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LayerMask IgnoreWhenSettingHeight;

    public event Action Dashed;
    
    private Vector3 _move;
    private Rigidbody _rigibody;
    private float _speed = 1200f;
    private PlayerLooker _playerLooker;
    private Highlight _highlight;
    private float _boostTime;
    private bool _boostThisFrame;
    private Animator _animator;
    private bool _frozen;
    [CanBeNull] private IUIController _uiController;
    private bool _grabThisFrame;
    private float _boostBlockTime;
    private bool _useThisFrame;
    private PlayerCrowdSurfer _crowdSurfer;
    private bool _targeting;

    void Start()
    {
        _rigibody = GetComponentInChildren<Rigidbody>();
        _playerLooker = GetComponentInChildren<PlayerLooker>();
        _highlight = GetComponentInChildren<Highlight>();
        _animator = GetComponentInChildren<Animator>();
        _crowdSurfer = GetComponentInChildren<PlayerCrowdSurfer>();

        TargetGroupForPlayers.Instance.AddPlayer(_rigibody.gameObject);
    }

    void Update()
    {
        if (_uiController != null)
        {
            HandleUIInput();
        }
        else if (!_frozen)
        {
            HandleMainInput();
        }

        _grabThisFrame = false;
        _useThisFrame = false;
    }

    private void HandleMainInput()
    {
        if (_crowdSurfer.IsLifted()) return;

        if (_boostThisFrame && _boostBlockTime <= 0f)
        {
            _rigibody.drag = 1f;

            _rigibody.AddForce(_move.normalized * 20f, ForceMode.Impulse);

            Sounds.Instance.PlayWooshSound(_rigibody.transform.position);

            _boostBlockTime = .7f;
            Dashed?.Invoke();
        }
        else
        {
            if (!_targeting && _move.magnitude > .75f)
            {
                _rigibody.drag = 7f;
                var boostFactor = 1f;

                var currentPosition = _rigibody.transform.position;
                var hits = Physics.OverlapSphere(currentPosition - Vector3.up * .5f, .3f)
                    .Where(hit => hit.CompareTag("Tile")).ToArray();
                if (hits.Length > 0)
                {
                    var tile = ClosestHit(hits);
                    _rigibody.AddForce((tile.transform.position - currentPosition).normalized * 900f * Time.deltaTime,
                        ForceMode.Acceleration);

                    boostFactor = 1.3f;
                }

                var force = _move * _speed * boostFactor * Time.deltaTime;
                _rigibody.AddForce(force, ForceMode.Acceleration);
            }
            else
            {
                _rigibody.drag = 10f;
            }
        }

        var rigibodyVelocity = _rigibody.velocity;
        var flatVelocity = new Vector2(rigibodyVelocity.x, rigibodyVelocity.z);
        if (flatVelocity.magnitude > .6f)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        if (_move.magnitude > .2f)
        {
            _playerLooker.OrientWith(_move);
            _highlight.MoveAlong(_move);
        }


        if (!_rigibody.isKinematic && _rigibody.position.y < 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(_rigibody.position, Vector3.down), out hit, 2f, ~IgnoreWhenSettingHeight))
            {
                var position = _rigibody.position;
                _rigibody.position = new Vector3(
                    position.x,
                    hit.point.y + .6f,
                    position.z
                );
            }
        }

        if (_boostBlockTime > 0f) _boostBlockTime -= Time.deltaTime;
        _boostThisFrame = false;
    }

    private void HandleUIInput()
    {
        if (_move.magnitude > .4f) _uiController.Move(_move);
        else _uiController.MoveReset();

        if (_useThisFrame) _uiController.PlayerSubmit();
    }

    public bool IsTargeting()
    {
        return _targeting;
    }

    void OnGrab(InputValue value)
    {
        _grabThisFrame = value.isPressed;
    }

    void OnTarget(InputValue value)
    {
        var pressAmount = value.Get<float>();
        _targeting = pressAmount > .5f;
    }

    void OnUse(InputValue value)
    {
        _useThisFrame = value.isPressed;
    }

    void OnMove(InputValue value)
    {
        var move = value.Get<Vector2>();
        _move = new Vector3(
            move.x,
            0,
            move.y
        );
    }

    void OnBoost(InputValue value)
    {
        if (value.isPressed)
        {
            _boostThisFrame = true;
            // _boostTime = Time.time;
        }
    }

    private GameObject ClosestHit(Collider[] hits)
    {
        GameObject nearestPlanet = null;
        float nearestDistance = float.MaxValue;
        float distance;

        foreach (Collider hit in hits)
        {
            var planet = hit.gameObject;
            distance = (_rigibody.transform.position - planet.transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlanet = planet;
            }
        }

        return nearestPlanet;
    }

    public void Freeze()
    {
        _frozen = true;
    }

    public void UnFreeze()
    {
        _frozen = false;
    }

    public void SetUIController(IUIController uiController)
    {
        _animator.SetBool("IsWalking", false);
        _uiController = uiController;
    }

    public void ClearUIController()
    {
        _uiController = null;
    }
}