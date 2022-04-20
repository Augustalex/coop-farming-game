using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugNet : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Holding = Animator.StringToHash("Holding");
    private static readonly int Swing = Animator.StringToHash("Swing");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetTrigger(Swing);
    }


    private void OnTriggerEnter(Collider other)
    {
        var seedItem = other.GetComponent<SeedItem>();
        if (seedItem)
        {
            Destroy(seedItem.gameObject);
        }
    }
}