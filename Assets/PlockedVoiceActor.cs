using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class PlockedVoiceActor : MonoBehaviour
{
    public AudioClip[] grabbedSounds;
    public AudioClip[] droppedSounds;

    void Start()
    {
        var item = GetComponent<PlayerItem>();
        item.Grabbed += OnGrabbed;
        item.Dropped += OnDropped;

        // GetComponent<WildCube>().Jumped += OnJumped;

        OnSpawned();
    }

    private void OnJumped()
    {
        if (Random.value < .02f) return;
        OnDropped();
    }

    public void OnGrabbed()
    {
        var sound = grabbedSounds[Random.Range(0, grabbedSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .5f);
    }

    public void OnDropped()
    {
        var sound = droppedSounds[Random.Range(0, droppedSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .5f);
    }

    public void OnRelocated()
    {
        var sound = droppedSounds[Random.Range(0, droppedSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .3f);
    }

    public void OnSpawned()
    {
        var sound = droppedSounds[Random.Range(0, droppedSounds.Length)];
        var sound2 = grabbedSounds[Random.Range(0, droppedSounds.Length)];
        Sounds.Instance.PlaySound(Random.value < .5f ? sound : sound2, transform.position, .3f);
    }
}