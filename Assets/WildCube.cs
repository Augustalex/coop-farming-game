using System;
using System.Linq;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class WildCube : MonoBehaviour
{
    private Rigidbody _body;
    private float _jumpCooldown;
    private Plant _plant;
    private SeedItem _seedItem;
    private PlayerItem _playerItem;
    private float _wantsWaterCooldown;
    private GameObject _target;

    public event Action Jumped;
    public event Action Evaded;
    public event Action Relocated;
    public event Action Thirsty;

    void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _plant = GetComponent<Plant>();
        _playerItem = GetComponent<PlayerItem>();

        _seedItem = GetComponent<SeedItem>();
    }

    void Update()
    {
        if (_plant && _plant.InSoil()) return;
        if (_playerItem && _playerItem.IsGrabbed()) return;

        if (_wantsWaterCooldown > 0)
        {
            _wantsWaterCooldown -= Time.deltaTime;
        }
        else if (_jumpCooldown < 0 && _seedItem)
        {
            var soils = Physics.OverlapSphere(_body.position, 2f)
                .Where(hit => hit.CompareTag("Soil") && hit.GetComponent<SoilBlock>().IsFree()).ToArray();
            if (soils.Length > 0)
            {
                var soil = soils[0];
                _seedItem.OnUseOnSoil(soil.GetComponent<SoilBlock>());
            }
        }

        if (_jumpCooldown < 0)
        {
            Jump(GetJumpDirection());
            Jumped?.Invoke();
        }
        else
        {
            _jumpCooldown -= Time.deltaTime;
        }
    }

    private void Jump(Vector3 jumpDirection)
    {
        _body.AddForce(jumpDirection * 3f + Vector3.up * 4f, ForceMode.Impulse);

        _jumpCooldown = Random.Range(4, 7);
    }

    public Vector3 GetJumpDirection()
    {
        if (_wantsWaterCooldown > 0)
        {
            var directionToTarget = (_target.transform.position - _body.position).normalized;
            return new Vector3(
                directionToTarget.x,
                0,
                directionToTarget.y
            );
        }
        else
        {
            var randomDirection = Random.insideUnitCircle;
            return new Vector3(
                randomDirection.x,
                0,
                randomDirection.y
            );
        }
    }

    public void WantsWater()
    {
        var rivers = Physics.OverlapSphere(_body.position, 30f).Where(hit => hit.CompareTag("River"))
            .OrderBy(_ => Random.value - .5f);
        if (rivers.Any())
        {
            var river = rivers.First();
            _target = river.gameObject;
        }

        _wantsWaterCooldown = 20f;

        Thirsty?.Invoke();
    }

    public void RunAwayFromDash(Vector3 playerPosition)
    {
        var direction = (_body.position - playerPosition).normalized;
        var flatDirection = new Vector3(direction.x, 0, direction.z);
        Jump(flatDirection);
        Evaded?.Invoke();
    }

    public void Relocate()
    {
        Jump(GetJumpDirection());
        Relocated?.Invoke();
    }
}