using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrowdSurfer : MonoBehaviour
{
    private Rigidbody _liftedBy;
    private Rigidbody _body;
    private float _cooldown;
    private PlayerLooker _looker;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _looker = GetComponent<PlayerLooker>();
    }

    void Update()
    {
        if (_liftedBy)
        {
            _body.position = _liftedBy.position + Vector3.up * 2f;
        }

        if (_cooldown > 0)
        {
            _body.AddForce(Vector3.down * 500f * Time.deltaTime, ForceMode.Acceleration);
            _cooldown -= Time.deltaTime;
        }
    }

    public void LiftedUpByAnotherPlayer(GameObject liftedBy)
    {
        _liftedBy = liftedBy.GetComponent<Rigidbody>();

        _body.isKinematic = true;
    }

    public bool IsLifted()
    {
        return _cooldown > 0 || _liftedBy != null;
    }

    public void Drop()
    {
        _body.isKinematic = false;

        var rawDirection = _liftedBy.transform.forward.normalized;
        _body.drag = 0;
        _body.AddForce(rawDirection * 9f + Vector3.up * 6f, ForceMode.Impulse);

        _liftedBy = null;
        _cooldown = 1.25f;
    }
}