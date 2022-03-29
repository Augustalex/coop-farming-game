using System;
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
    private ItemGhost _ghost;
    private SmartGhost _smartGhots;

    public static readonly Vector3 SelectColumn = new Vector3(
        .4f,
        5f,
        .4f
    );

    private PlayerItem _playerItem;

    private void Update()
    {
        if (_ghost)
        {
            var highlightPosition = highlight.transform.position;
            _ghost.Move(highlightPosition);
        }

        if (_smartGhots)
        {
            var highlightPosition = highlight.transform.position;
            _smartGhots.OnMove(highlightPosition);
        }

        if (_playerItem)
        {
            var highlightPosition = highlight.transform.position;
            _playerItem.Provoke(highlightPosition);
        }
    }

    void OnRotate(InputValue value)
    {
        if (value.isPressed)
        {
            if (_ghost)
            {
                _ghost.Rotate();
            }
        }
    }

    void OnGrab(InputValue value)
    {
        if (value.isPressed)
        {
            if (_grabbing)
            {
                if (!CanUseWithGrabAction())
                {
                    Drop();
                }
            }
            else
            {
                if (highlight.HasHighlightedItem())
                {
                    var item = highlight.GetHighlightedItem();
                    // var hit = selfHits.Length > 0 ? selfHits[0] : highlightHits[0];
                    _grabbing = item;

                    _playerItem = _grabbing.GetComponent<PlayerItem>();
                    _playerItem.GrabbedBy(this);

                    _grabbing.transform.SetParent(body.transform);
                    _grabbing.transform.position = body.transform.position + Vector3.up * 1.25f;

                    var plant = _grabbing.GetComponent<Plant>();
                    if (plant)
                    {
                        plant.Grabbed();
                    }

                    var ghost = _grabbing.GetComponent<ItemGhost>();
                    if (ghost)
                    {
                        _ghost = ghost;
                    }

                    var smartGhost = _grabbing.GetComponent<SmartGhost>();
                    if (smartGhost)
                    {
                        _smartGhots = smartGhost;
                    }

                    Sounds.Instance.PlayPickupItemSound(transform.position);
                }

                // var alignedBodyPosition = Highlight.AlignToGrid(body.transform.position);
                // var selfHits = Physics
                //     .OverlapBox(alignedBodyPosition, SelectColumn)
                //     .Where(Highlight.ShouldHighlightItem)
                //     .ToArray();
                //
                // var highlightHits = Physics
                //     .OverlapBox(highlight.transform.position, SelectColumn)
                //     .Where(Highlight.ShouldHighlightItem)
                //     .ToArray();
                //
                // if (selfHits.Length > 0 || highlightHits.Length > 0)
                // {
                //     var hit = selfHits.Length > 0 ? selfHits[0] : highlightHits[0];
                //     _grabbing = hit.gameObject;
                //     _grabbingBody = hit.attachedRigidbody;
                //     _grabbingBody.isKinematic = true;
                //
                //     _playerItem = _grabbing.GetComponent<PlayerItem>();
                //     _playerItem.GrabbedBy(this);
                //
                //     _grabbing.transform.SetParent(body.transform);
                //     _grabbing.transform.position = body.transform.position + Vector3.up * 1.25f;
                //
                //
                //     var plant = _grabbing.GetComponent<Plant>();
                //     if (plant)
                //     {
                //         plant.Grabbed();
                //     }
                //
                //     var ghost = _grabbing.GetComponent<ItemGhost>();
                //     if (ghost)
                //     {
                //         _ghost = ghost;
                //     }
                //
                //     var smartGhost = _grabbing.GetComponent<SmartGhost>();
                //     if (smartGhost)
                //     {
                //         _smartGhots = smartGhost;
                //     }
                //
                //     Sounds.Instance.PlayPickupItemSound(transform.position);
                // }
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

        // Drop at highlight
        // _grabbing.transform.position = highlight.transform.position + Vector3.up;

        // Drop where player stands
        _grabbing.transform.position = Highlight.AlignToGrid(body.transform.position) + Vector3.up;
        _grabbing.transform.rotation = Quaternion.identity;
        _playerItem = _grabbing.GetComponent<PlayerItem>();
        _playerItem.WasDropped();

        _grabbing = null;
        _playerItem = null;
        _ghost = null;
        _smartGhots = null;

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

    public bool CanUseWithGrabAction()
    {
        if (!HasItem()) return false;

        var useOnSoil = _grabbing.GetComponent<UseOnSoil>();
        return useOnSoil && useOnSoil.HoverOnSoil(highlight.transform.position);
    }

    public void StealGrabbedItem()
    {
        Drop();
    }
}