using System;
using System.Collections.Generic;
using System.Linq;
using Game.Ghost;
using UnityEngine;

namespace Game
{
    public class SmartItemGhost : MonoBehaviour
    {
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
            Free,
            ByRiver
        }

        public enum SoilConstraints
        {
            FreeSoil,
            IsDry,
            HasSeeds
        }

        private HashSet<GhostToggles> _toggles;
        private HashSet<GrassConstraints> _grassConstraints;
        private HashSet<SoilConstraints> _soilConstraints;
        private GhostController _ghostController;

        private CountData _countData;
        private bool _canPlace;
        private bool _visible;
        private bool _deactivated;

        void Start()
        {
            _playerItem = GetComponent<PlayerItem>();

            _playerItem.Dropped += OnDrop;
            _playerItem.Escaped += OnDrop;

            _playerItem.Grabbed += OnGrabbed;
            _playerItem.Provoked += OnMove;

            _countData = GetComponent<CountData>();
        }

        private void Update()
        {
            // if (_deactivated)
            // {
            //     Hide();
            // }
        }

        public void ApplyConfiguration(ItemGhostConfiguration itemGhostConfiguration)
        {
            _toggles = new HashSet<GhostToggles>(itemGhostConfiguration.toggles);
            _grassConstraints = new HashSet<GrassConstraints>(itemGhostConfiguration.grassConstraints);
            _soilConstraints = new HashSet<SoilConstraints>(itemGhostConfiguration.soilConstraints);

            if (_ghost) Destroy(_ghost);
            _ghost = Instantiate(itemGhostConfiguration.template);
            _ghostController = _ghost.GetComponent<GhostController>();

            if (_visible) Show();
            else Hide();
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

        public void OnMove(Vector3 highlightPosition)
        {
            if (_deactivated) return;
            if (!_ghost) return;

            var hits = Physics.RaycastAll(new Ray(highlightPosition, Vector3.down), 3f)
                .Where(hit => hit.collider.CompareTag("Bush") || hit.collider.GetComponent<Interactable>()).ToArray();
            if (hits.Length > 0)
            {
                var raycastHit = hits[0];

                var canPlace = !_countData || _countData.count > 0;
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
            if (_deactivated) return false;
            if (!_ghost) return false;

            var hits = Physics.RaycastAll(new Ray(position, Vector3.down), 3f)
                .Where(hit => hit.collider.CompareTag("Bush") || hit.collider.GetComponent<Interactable>()).ToArray();
            if (hits.Length <= 0) return false;

            var raycastHit = hits[0];
            var canPlace = !_countData || _countData.count > 0;
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

            if (_grassConstraints.Contains(GrassConstraints.Free))
            {
                if (!hitCollider.GetComponent<GrassBlock>().IsFree())
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

        private void Hide()
        {
            if (!_ghost) return;

            _visible = false;
            _ghost.SetActive(false);
        }

        private void Show()
        {
            if (!_ghost) return;

            _visible = false;
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

        public Transform GhostTransform()
        {
            return _ghost.transform;
        }

        public void Rotate()
        {
            _ghost.transform.RotateAround(transform.position, Vector3.up, 90f);
        }

        public void Deactivate()
        {
            _deactivated = true;
            Hide();
        }

        public void Activate()
        {
            _deactivated = false;
            Show();
        }
    }
}