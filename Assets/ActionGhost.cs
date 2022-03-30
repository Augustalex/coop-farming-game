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
        Plant
    };

    public GhostToggles[] toggles; // Cannot be updated at runtime
    private HashSet<GhostToggles> _toggles;

    private GhostController _ghostController;

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
        Show();
    }

    private void OnDrop()
    {
        Hide();
    }

    private void OnProvoked(Vector3 highlightPosition)
    {
        var hits = Physics.RaycastAll(new Ray(highlightPosition + Vector3.up * 2f, Vector3.down), 3f)
            .Where(hit =>
            {
                if (_toggles.Contains(GhostToggles.Weeds) && hit.collider.CompareTag("Weeds")) return true;
                if (_toggles.Contains(GhostToggles.Plant) && hit.collider.CompareTag("Goods")) return true;
                if (_toggles.Contains(GhostToggles.Bush) && hit.collider.CompareTag("Bush")) return true;
                return false;
            }).ToArray(); // We could compare all the toggles here already? A small optimization perhaps.
        if (hits.Length > 0)
        {
            var raycastHit = hits[0];

            Show();
            _ghostController.CanPlace();

            _ghost.transform.position = raycastHit.collider.transform.position + Vector3.up;
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
}