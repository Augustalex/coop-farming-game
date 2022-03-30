using System;
using System.Linq;
using Game;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private PlayerLooker _playerBoody;
    private MeshRenderer _meshRenderer;
    private GameObject _lifted;

    public static readonly float HighlightLiftOffset = 1.5f;
    private GameObject _highlightedItem;

    private void Awake()
    {
        _playerBoody = transform.parent.GetComponentInChildren<PlayerLooker>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void MoveAlong(Vector3 move)
    {
        var alignedBodyPosition = AlignToGrid(_playerBoody.transform.position);
        var itemsBeneathPlayerBody = Physics
            .OverlapBox(alignedBodyPosition, PlayerGrabber.SelectColumn)
            .Where(ShouldHighlightItem)
            .ToArray();

        var target = itemsBeneathPlayerBody.Length > 0
            ? alignedBodyPosition
            : alignedBodyPosition + move.normalized * .75f;
        
        // Move highlight to target
        transform.position = AlignToGrid(target);

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

        if (itemsBeneathPlayerBody.Length > 0)
        {
            var selfHit = itemsBeneathPlayerBody[0];
            _highlightedItem = selfHit.gameObject;
            _meshRenderer.enabled = true;

            DisableLifting();
            EnableLiftingOn(selfHit);
        }
        else
        {
            var itemsUnderHighlight = Physics.OverlapBox(transform.position, PlayerGrabber.SelectColumn)
                .Where(ShouldHighlightItem)
                .ToArray();
            if (itemsUnderHighlight.Length > 0)
            {
                var boxHit = itemsUnderHighlight[0];
                _highlightedItem = boxHit.gameObject;

                DisableLifting();
                EnableLiftingOn(boxHit);

                _meshRenderer.enabled = true;
            }
            else
            {
                _highlightedItem = null;
                DisableLifting();

                _meshRenderer.enabled = false;
            }
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