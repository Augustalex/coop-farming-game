using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class BugNet : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Holding = Animator.StringToHash("Holding");
    private static readonly int Swing = Animator.StringToHash("Swing");
    private CatcherNet _catcherNet;
    private float _swingUntil;
    private readonly List<SeedItem> _seedsCaught = new List<SeedItem>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _catcherNet = GetComponentInChildren<CatcherNet>();
    }

    private void Start()
    {
        var playerItem = GetComponent<PlayerItem>();
        playerItem.UseItem += OnUse;
        playerItem.Grabbed += OnGrabbed;
        playerItem.Dropped += OnDropped;
    }

    private void OnDropped()
    {
        _animator.SetBool(Holding, false);
    }

    private void OnGrabbed()
    {
        _animator.SetBool(Holding, true);
    }

    public void OnUse(Vector3 highlightPosition)
    {
        if (Time.time < _swingUntil) return;

        _animator.SetTrigger(Swing);
        _swingUntil = Time.time + 1;
        Sounds.Instance.PlaySwingSound(_catcherNet.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time > _swingUntil) return;

        var seedItem = other.GetComponent<SeedItem>();
        if (seedItem)
        {
            var sameTypeAsOnesAlreadyCaught = _seedsCaught.Count > 0 && _seedsCaught[0].CompareToSeedItem(seedItem);
            if (_seedsCaught.Count == 0 || sameTypeAsOnesAlreadyCaught)
            {
                var alreadyCaught = _seedsCaught.Contains(seedItem);
                if (!alreadyCaught)
                {
                    var seedItemTransform = seedItem.transform;
                    seedItemTransform.SetParent(_catcherNet.transform);
                    seedItemTransform.localPosition = Vector3.zero + Random.insideUnitSphere * .3f;
                    seedItemTransform.rotation = Random.rotation;

                    seedItem.GetComponent<PlayerItem>().GrabbedByNonPlayer(_catcherNet.transform);

                    _seedsCaught.Add(seedItem);

                    Sounds.Instance.PlayUseBucketSound(_catcherNet.transform.position);
                }
            }
            else
            {
                var wildCube = seedItem.GetComponent<WildCube>();
                if (wildCube)
                {
                    wildCube.RunAwayFromDash(_catcherNet.transform.position);
                }
            }
        }
    }
}