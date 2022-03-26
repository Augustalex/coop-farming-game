using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabber : MonoBehaviour
{
    public Highlight highlight;
    public PlayerLooker body;
    private GameObject _grabbing;

    void OnGrab(InputValue value)
    {
        if (value.isPressed)
        {
            if (_grabbing)
            {
                Drop();
            }
            else
            {
                var hits = Physics.OverlapSphere(highlight.transform.position, .5f)
                    .Where(hit => hit.GetComponent<PlayerItem>()).ToArray();
                if (hits.Length > 0)
                {
                    var hit = hits[0];
                    _grabbing = hit.gameObject;

                    var playerItem = _grabbing.GetComponent<PlayerItem>();
                    playerItem.GrabbedBy(this);

                    _grabbing.transform.SetParent(body.transform);
                    _grabbing.transform.position = body.transform.position + Vector3.up;

                    _grabbing.GetComponent<Rigidbody>().isKinematic = true;

                    var plant = _grabbing.GetComponent<Plant>();
                    if (plant)
                    {
                        plant.Grabbed();
                    }

                    Sounds.Instance.PlayPickupItemSound(transform.position);
                }
            }
        }
    }

    private void Drop()
    {
        if (!_grabbing) return;

        var grabbedPlayer = _grabbing.GetComponent<PlayerController>();
        if (grabbedPlayer)
        {
            grabbedPlayer.PickedUp();
        }

        _grabbing.transform.SetParent(null);
        _grabbing.transform.position = highlight.transform.position + Vector3.up;
        _grabbing.transform.rotation = Quaternion.identity;
        _grabbing.GetComponentInChildren<Rigidbody>().isKinematic = false;

        _grabbing = null;

        Sounds.Instance.PlayDropItemSound(transform.position);
    }

    public bool HasItem()
    {
        return _grabbing != null;
    }

    public void UseItem()
    {
        _grabbing.GetComponent<PlayerItem>().Use(highlight.transform.position);
    }

    public void StealGrabbedItem()
    {
        Drop();
    }
}