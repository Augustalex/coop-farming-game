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

    private Vector3 _move;
    private Rigidbody _rigibody;
    private float _speed = 1200f;
    private PlayerLooker _playerLooker;
    private Highlight _highlight;
    private float _boostTime;
    private bool _boostThisFrame;
    private float _originalHeight;
    private Animator _animator;
    private bool _frozen;
    [CanBeNull] private IUIController _uiController;
    private bool _grabThisFrame;
    private float _boostBlockTime;
    private bool _useThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        _rigibody = GetComponentInChildren<Rigidbody>();
        _playerLooker = GetComponentInChildren<PlayerLooker>();
        _highlight = GetComponentInChildren<Highlight>();
        _animator = GetComponentInChildren<Animator>();

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
        if (_boostThisFrame && _boostBlockTime <= 0f)
        {
            _rigibody.drag = 1f;

            _rigibody.AddForce(_move.normalized * 20f, ForceMode.Impulse);

            Sounds.Instance.PlayWooshSound(transform.position);

            _boostBlockTime = .8f;
        }
        else
        {
            if (_move.magnitude < .45f)
            {
                _rigibody.drag = 10f;
            }
            else
            {
                _rigibody.drag = 7f;
            }

            if (_move.magnitude > .6f)
            {
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
        }

        if (_rigibody.velocity.magnitude > .5f)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        if (_move.magnitude > .25f)
        {
            _playerLooker.OrientWith(_move);
            _highlight.MoveAlong(_move);
        }


        if (!_rigibody.isKinematic)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(_rigibody.position, Vector3.down), out hit, 2f, ~IgnoreWhenSettingHeight))
            {
                var position = transform.position;
                transform.position = new Vector3(
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

    void OnGrab(InputValue value)
    {
        _grabThisFrame = value.isPressed;
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

    public void Dropped(Vector3 position)
    {
        transform.position = new Vector3(
            position.x,
            _originalHeight,
            position.z
        );
        _rigibody.isKinematic = false;
    }

    public void PickedUp()
    {
        _originalHeight = transform.position.y;
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