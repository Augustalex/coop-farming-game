using System;
using System.Linq;
using System.Net.Configuration;
using Game;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private PlayerLooker _playerBoody;
    private MeshRenderer _meshRenderer;
    private GameObject _lifted;

    public static readonly float HighlightLiftOffset = 1.5f;
    private GameObject _highlightedItem;
    private PlayerGrabber _playerGrabber;

    private void Awake()
    {
        _playerBoody = transform.parent.GetComponentInChildren<PlayerLooker>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _playerGrabber = GetComponentInParent<PlayerGrabber>();
    }

    public void MoveAlong(Vector3 move)
    {
        var currentPlayerPosition = AlignToGrid(_playerBoody.transform.position);
        var toTry = new[]
        {
            currentPlayerPosition,
            currentPlayerPosition + move.normalized * 1.5f,
            currentPlayerPosition + Vector3.forward,
            currentPlayerPosition + Vector3.right,
            currentPlayerPosition + Vector3.left,
            currentPlayerPosition + Vector3.back
        };

        if (!TryAll(toTry))
        {
            _highlightedItem = null;
            DisableLifting();

            _meshRenderer.enabled = false;
        }


        // var itemsBeneathPlayerBody = Physics
        //     .OverlapBox(currentPlayerPosition, PlayerGrabber.SelectColumn)
        //     .Where(ShouldHighlightItem)
        //     .ToArray();
        //
        // var canSelectItemUnderPlayer = !_playerGrabber.HasItem() && itemsBeneathPlayerBody.Length > 0;
        // var shouldHighlightCurrentPosition =
        //     canSelectItemUnderPlayer || _playerGrabber.ValidLocation(currentPlayerPosition);
        // if (shouldHighlightCurrentPosition)
        // {
        //     transform.position = AlignToGrid(currentPlayerPosition); // Move highlight to target
        //
        //     if (canSelectItemUnderPlayer)
        //     {
        //         var selfHit = itemsBeneathPlayerBody[0];
        //         _highlightedItem = selfHit.gameObject;
        //         _meshRenderer.enabled = true;
        //         DisableLifting();
        //         EnableLiftingOn(selfHit);
        //     }
        //     else
        //     {
        //         DisableLifting();
        //         // There is probably an available Item action
        //     }
        // }
        // else
        // {
        //     var aTileAwayFromPlayer = currentPlayerPosition + move.normalized * 1f;
        //     transform.position = AlignToGrid(aTileAwayFromPlayer); // Move highlight to target
        //
        //     var itemsUnderHighlight = Physics.OverlapBox(transform.position, PlayerGrabber.SelectColumn)
        //         .Where(ShouldHighlightItem)
        //         .ToArray();
        //     if (itemsUnderHighlight.Length > 0)
        //     {
        //         var boxHit = itemsUnderHighlight[0];
        //         _highlightedItem = boxHit.gameObject;
        //
        //         DisableLifting();
        //         EnableLiftingOn(boxHit);
        //
        //         _meshRenderer.enabled = true;
        //     }
        //     else
        //     {
        //         _highlightedItem = null;
        //         DisableLifting();
        //
        //         _meshRenderer.enabled = false;
        //     }
        // }

        AlignHeightOfHighlight(); // Need to be after the highlight has been repositioned
    }

    private void OnDrawGizmos()
    {
        // var currentPlayerPosition = AlignToGrid(_playerBoody.transform.position);
        // var toTry = new[]
        // {
        //     currentPlayerPosition,
        //     currentPlayerPosition + _lastMove.normalized * 1.5f,
        //     currentPlayerPosition + Vector3.forward,
        //     currentPlayerPosition + Vector3.right,
        //     currentPlayerPosition + Vector3.left,
        //     currentPlayerPosition + Vector3.back
        // };
        //
        // foreach (var vector3 in toTry)
        // {
        //     Gizmos.DrawWireCube(AlignToGrid(vector3), Vector3.one * .8f);
        // }
    }

    private bool TryAll(Vector3[] toTry)
    {
        foreach (var newPosition in toTry)
        {
            if (TryHighlight(newPosition))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryHighlight(Vector3 position)
    {
        // transform.position = AlignToGrid(position); // Move highlight to target

        var itemsUnderHighlight = Physics.OverlapBox(position, PlayerGrabber.SelectColumn)
            .Where(ShouldHighlightItem)
            .ToArray();
        var canSelectItemUnderPlayer = !_playerGrabber.HasItem() && itemsUnderHighlight.Length > 0;
        var validLocation = _playerGrabber.ValidLocation(position);
        var shouldHighlightCurrentPosition =
            canSelectItemUnderPlayer || validLocation;

        if (shouldHighlightCurrentPosition)
        {
            transform.position = AlignToGrid(position); // Move highlight to target

            if (canSelectItemUnderPlayer)
            {
                var selfHit = itemsUnderHighlight[0];
                _highlightedItem = selfHit.gameObject;
                _meshRenderer.enabled = true;
                DisableLifting();
                EnableLiftingOn(selfHit);
            }
            else
            {
                DisableLifting();
                // There is probably an available Item action - that is why we dont need to lift any item
            }

            return true;
        }

        return false;
    }

    private void AlignHeightOfHighlight()
    {
        var groundUnderHighlight = Physics.RaycastAll(new Ray(transform.position + Vector3.up, Vector3.down), 3f)
            .Where(hit => !hit.collider.isTrigger && hit.collider.GetComponent<Interactable>()).ToArray();
        if (groundUnderHighlight.Length > 0)
        {
            var ground = groundUnderHighlight[0].collider.transform.position;
            var currentPosition = transform.position;
            transform.position = new Vector3(
                currentPosition.x,
                ground.y + .7f,
                currentPosition.z);
        }
    }

    public bool HasHighlightedItem()
    {
        return _highlightedItem != null;
    }

    public GameObject GetHighlightedItem()
    {
        return _highlightedItem;
    }

    private void EnableLiftingOn(Collider boxHit)
    {
        boxHit.GetComponent<PlayerItem>().LiftUp(PlayerItem.LifterIdentifier.Highlight, HighlightLiftOffset);
        _lifted = boxHit.gameObject;
    }

    private void DisableLifting()
    {
        if (_lifted)
        {
            _lifted.GetComponent<PlayerItem>().StopLifting(PlayerItem.LifterIdentifier.Highlight);
        }

        _lifted = null;
    }

    public static bool ShouldHighlightItem(Collider hit)
    {
        if (hit.isTrigger) return false;
        var playerItem = hit.GetComponent<PlayerItem>();
        if (playerItem)
        {
            return !playerItem.IsGrabbed();
        }

        return false;
    }

    public static Vector3 AlignToGrid(Vector3 point)
    {
        return new Vector3(
            Mathf.Round(point.x),
            Mathf.Round(point.y),
            Mathf.Round(point.z)
        );
    }
}