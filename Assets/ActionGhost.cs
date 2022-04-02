using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class ActionGhost : MonoBehaviour
{
    public GameObject ghostTemplate;
    private GameObject _ghost;
    private PlayerItem _playerItem;

    [System.Serializable]
    public enum GhostToggles
    {
        Weeds,
        Bush,
        Plant,
        Tile,
        Fence,
        WaterUtility
    };

    public GhostToggles[] toggles; // Cannot be updated at runtime
    private HashSet<GhostToggles> _toggles;

    private GhostController _ghostController;
    private bool _deactivated;

    void Start()
    {
        _ghost = Instantiate(ghostTemplate);
        _ghostController = _ghost.GetComponent<GhostController>();

        _toggles = new HashSet<GhostToggles>(toggles);

        _playerItem = GetComponent<PlayerItem>();
        _playerItem.Dropped += OnDrop;
        _playerItem.Grabbed += OnGrabbed;
        _playerItem.Provoked += OnProvoked;

        Hide();
    }

    private void OnGrabbed()
    {
        if (_deactivated) return;
        Show();
    }

    private void OnDrop()
    {
        if (_deactivated) return;
        Hide();
    }

    private void OnProvoked(Vector3 highlightPosition)
    {
        if (_deactivated) return;

        var hits = HitsForPosition(highlightPosition);
        if (hits.Length > 0)
        {
            var raycastHit = hits[0];

            Show();
            _ghostController.CanPlace();

            _ghost.transform.position = raycastHit.transform.position + Vector3.up;
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        _ghost.SetActive(false);
    }

    private void Show()
    {
        _ghost.SetActive(true);
    }

    public bool IsValidLocation(Vector3 position)
    {
        if (_deactivated) return false;

        return HitsForPosition(position).Length > 0;
    }

    public Collider[] HitsForPosition(Vector3 highlightPosition)
    {
        var hits = Physics.OverlapSphere(highlightPosition + Vector3.up, 1.5f)
            .Where(hit =>
            {
                if (_toggles.Contains(GhostToggles.Weeds) && hit.CompareTag("Weeds")) return true;
                if (_toggles.Contains(GhostToggles.Plant) && hit.CompareTag("Goods")) return true;
                if (_toggles.Contains(GhostToggles.Bush) && hit.CompareTag("Bush")) return true;
                if (_toggles.Contains(GhostToggles.Tile) && hit.CompareTag("Tile")) return true;
                if (_toggles.Contains(GhostToggles.Fence) && hit.CompareTag("Fence")) return true;
                if (_toggles.Contains(GhostToggles.WaterUtility) && hit.CompareTag("Sprinkler")) return true;
                return false;
            }).ToArray(); // We could compare all the toggles here already? A small optimization perhaps.
        return hits;
    }

    public void Deactivate()
    {
        _deactivated = true;
    }

    public void Activate()
    {
        _deactivated = false;
    }
}