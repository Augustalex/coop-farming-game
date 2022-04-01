using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrampledVoiceActor : MonoBehaviour
{
    public AudioClip[] deathSounds;
    public AudioClip[] sounds;

    void Start()
    {
        GetComponent<SeedGrowth>().Died += OnDeath;
        GetComponent<SeedGrowth>().WasHurt += OnHurt;
    }

    private void OnDeath()
    {
        var sound = deathSounds[Random.Range(0, deathSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .8f);
    }

    private void OnHurt()
    {
        var sound = sounds[Random.Range(0, sounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .8f);
    }
}