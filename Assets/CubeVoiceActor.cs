using Game;
using UnityEngine;

public class CubeVoiceActor : MonoBehaviour
{
    public AudioClip[] grabbedSounds;
    public AudioClip[] droppedSounds;
    public AudioClip[] annoyedSounds;
    public AudioClip[] deathSounds;

    void Awake()
    {
        var seedGrowth = GetComponent<SeedGrowth>();
        if (seedGrowth)
        {
            seedGrowth.Died += OnDeath;
            seedGrowth.WasHurt += OnHurt;
        }

        var item = GetComponent<PlayerItem>();
        if (item)
        {
            item.Grabbed += OnGrabbed;
            item.Dropped += OnDropped;
        }

        var wildCube = GetComponent<WildCube>();
        if (wildCube)
        {
            // wildCube.Jumped += OnJumped;
            wildCube.Evaded += OnJumped;
            wildCube.Relocated += OnRelocated;
            wildCube.Thirsty += OnDeath;
        }
    }

    void Start()
    {
        if (Time.time < 5) return; // Do not make sounds for things spawned when game is just started
        OnSpawned();
    }

    public void OnDeath()
    {
        var sound = deathSounds[Random.Range(0, deathSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .8f);
    }

    private void OnHurt()
    {
        var sound = annoyedSounds[Random.Range(0, annoyedSounds.Length)];
        Sounds.Instance.PlaySound(sound, transform.position, .8f);
    }

    private void OnJumped()
    {
        if (Random.value < .25f) return;
        OnHurt();
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