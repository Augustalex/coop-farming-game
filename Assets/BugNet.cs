using System.Collections.Generic;
using Game;
using UnityEngine;

public class BugNet : MonoBehaviour
{
    private const float SwingCatchTimeframe = .3f;
    private const float SwingCooldownTime = 1.5f;
    private static readonly int Holding = Animator.StringToHash("Holding");
    private static readonly int Swing = Animator.StringToHash("Swing");
    private readonly Stack<SeedItem> _seedsCaught = new Stack<SeedItem>();
    private Animator _animator;
    private CatcherNet _catcherNet;
    private float _noEscapeUntil;
    private float _swingUntil;

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
        playerItem.Escaped += OnDropped;
    }

    private void Update()
    {
        if (_seedsCaught.Count > 0)
            if (Time.time > _noEscapeUntil)
            {
                if (Random.value < .5f) SeedEscaped();

                _noEscapeUntil = Time.time + Random.Range(2, 5);
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time > _swingUntil) return;

        var seedItem = other.GetComponent<SeedItem>();
        if (seedItem)
        {
            var alreadyCaught = _seedsCaught.Contains(seedItem);
            if (!alreadyCaught)
            {
                var sameTypeAsOnesAlreadyCaught =
                    _seedsCaught.Count > 0 && _seedsCaught.Peek().CompareToSeedItem(seedItem);
                if (_seedsCaught.Count == 0 || sameTypeAsOnesAlreadyCaught)
                {
                    var seedItemTransform = seedItem.transform;
                    seedItemTransform.SetParent(_catcherNet.transform);
                    seedItemTransform.localPosition = Vector3.zero + Random.insideUnitSphere * .3f;
                    seedItemTransform.rotation = Random.rotation;

                    seedItem.GetComponent<PlayerItem>().GrabbedByNonPlayer(_catcherNet.gameObject);

                    _seedsCaught.Push(seedItem);

                    Sounds.Instance.PlayUseBucketSound(_catcherNet.transform.position);
                }
            }
            else
            {
                var wildCube = seedItem.GetComponent<WildCube>();
                if (wildCube) wildCube.RunAwayFromDash(_catcherNet.transform.position);
            }
        }

        var seedSack = other.GetComponent<SeedSack>();
        if (seedSack && _seedsCaught.Count > 0)
            foreach (var seed in _seedsCaught)
                if (seedSack.CanTransferSeed(seed.gameObject))
                    seedSack.TransferSeeds(_seedsCaught);
                else
                    SeedEscape(seed.gameObject);
    }

    private void SeedEscaped()
    {
        if (_seedsCaught.Count == 0) return;

        var seed = _seedsCaught.Pop();
        SeedEscape(seed.gameObject);
    }

    private void SeedEscape(GameObject seed)
    {
        var seedItemTransform = seed.transform;
        seedItemTransform.SetParent(null);
        seedItemTransform.localPosition = Vector3.zero;
        seedItemTransform.position = _catcherNet.transform.position + Vector3.up;
        seedItemTransform.rotation = Quaternion.identity;

        seed.GetComponent<PlayerItem>().Escape();
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
        if (Time.time < _swingUntil + SwingCooldownTime) return;

        _animator.SetTrigger(Swing);
        _swingUntil = Time.time + SwingCatchTimeframe;
        Sounds.Instance.PlaySwingSound(_catcherNet.transform.position);
    }
}