using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Ghost;
using UnityEngine;

public class SmartGhost : MonoBehaviour
{
    public GameObject ghostTemplate;
    private GameObject _ghost;
    private PlayerItem _playerItem;

    [System.Serializable]
    public enum GhostToggles
    {
        Soil,
        Grass,
        Bush
    };

    public enum GrassConstraints
    {
        NoPlants,
        ByRiver
    }

    public enum SoilConstraints
    {
        FreeSoil,
        IsDry,
        HasSeeds
    }


    public GhostToggles[] toggles; // Cannot be updated at runtime
    private HashSet<GhostToggles> _toggles;

    public GrassConstraints[] grassConstraints; // Cannot be updated at runtime
    private HashSet<GrassConstraints> _grassConstraints;

    public SoilConstraints[] soilConstraints; // Cannot be updated at runtime
    private HashSet<SoilConstraints> _soilConstraints;

    private GhostController _ghostController;

    public Func<bool> CheckIfCanPlace = () => true;

    // For cache logic
    private float _cachedUntil;
    private bool _cachedCanPlace;

    private CountData _countData;
    private bool _canPlace;

    void Start()
    {
        _ghost = Instantiate(ghostTemplate);
        _ghostController = _ghost.GetComponent<GhostController>();

        _toggles = new HashSet<GhostToggles>(toggles);
        _grassConstraints = new HashSet<GrassConstraints>(grassConstraints);
        _soilConstraints = new HashSet<SoilConstraints>(soilConstraints);

        _playerItem = GetComponent<PlayerItem>();
        _playerItem.Dropped += OnDrop;
        _playerItem.Grabbed += OnGrabbed;

        _countData = GetComponent<CountData>();
    }

    private void OnGrabbed()
    {
        Show();
    }

    private void OnDrop()
    {
        Hide();
    }

    public void OnMove(Vector3 highlightPosition)
    {
        // WARNING: This method has to keep in sync with "IsValidLocation"

        var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
            .Where(hit => hit.collider.CompareTag("Bush") || hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length > 0)
        {
            var raycastHit = hits[0];

            var hasItemsToPlaceOrNoCounter = !_countData || _countData.count > 0;
            var canPlace = hasItemsToPlaceOrNoCounter && CheckIfCanPlacedRecently();
            if (!canPlace)
            {
                Hide();
            }
            else
            {
                Show();

                var hitCollider = raycastHit.collider;
                if (CompareToToggles(hitCollider) && PassesConstraints(hitCollider))
                {
                    _canPlace = true;
                    _ghostController.CanPlace();
                }
                else
                {
                    _canPlace = false;
                    _ghostController.CanNotPlace();
                }
            }

            _ghost.transform.position = highlightPosition;
        }
        else
        {
            Hide();
        }
    }

    public bool Activated()
    {
        return _canPlace;
    }

    public bool IsValidLocation(Vector3 position)
    {
        var hits = Physics.RaycastAll(new Ray(position, Vector3.down), 3f)
            .Where(hit => hit.collider.CompareTag("Bush") || hit.collider.GetComponent<Interactable>()).ToArray();
        if (hits.Length <= 0) return false;

        var raycastHit = hits[0];
        var hasItemsToPlaceOrNoCounter = !_countData || _countData.count > 0;
        var canPlace = hasItemsToPlaceOrNoCounter && CheckIfCanPlacedRecently();
        if (!canPlace)
        {
            return false;
        }

        var hitCollider = raycastHit.collider;
        return CompareToToggles(hitCollider) && PassesConstraints(hitCollider);
    }

    private bool PassesConstraints(Collider hitCollider)
    {
        if (_grassConstraints.Contains(GrassConstraints.NoPlants))
        {
            if (hitCollider.GetComponent<GrassBlock>().HasPlant())
            {
                return false;
            }
        }

        if (_grassConstraints.Contains(GrassConstraints.ByRiver))
        {
            var waterHits = Physics.OverlapSphere(hitCollider.gameObject.transform.position, .6f)
                .Where(hit => hit.CompareTag("River"))
                .ToArray();
            if (waterHits.Length == 0)
            {
                return false;
            }
        }

        if (_soilConstraints.Contains(SoilConstraints.FreeSoil))
        {
            if (!hitCollider.GetComponent<SoilBlock>().IsFree())
            {
                return false;
            }
        }

        if (_soilConstraints.Contains(SoilConstraints.IsDry))
        {
            if (!hitCollider.GetComponent<SoilBlock>().IsDry())
            {
                return false;
            }
        }

        if (_soilConstraints.Contains(SoilConstraints.HasSeeds))
        {
            if (!hitCollider.GetComponent<SoilBlock>().HasSeed())
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIfCanPlacedRecently()
    {
        // No cache
        return CheckIfCanPlace();

        // With cache logic
        // if (Time.time < _cachedUntil) return _cachedCanPlace;
        //
        // var canPlace = CheckIfCanPlace();
        // _cachedCanPlace = canPlace;
        // _cachedUntil = Time.time + .25f;
        //
        // return canPlace;
    }

    private void Hide()
    {
        _ghost.SetActive(false);
    }

    private void Show()
    {
        _ghost.SetActive(true);
    }

    private bool CompareToToggles(Collider collider)
    {
        return ((_toggles.Contains(GhostToggles.Grass) && collider.CompareTag("Grass"))
                || (_toggles.Contains(GhostToggles.Soil) && collider.CompareTag("Soil")))
               || (_toggles.Contains(GhostToggles.Bush) && collider.CompareTag("Bush"));
    }

    void OnDestroy()
    {
        Destroy(_ghost);
    }
}