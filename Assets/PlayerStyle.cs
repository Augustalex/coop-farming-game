using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStyle : MonoBehaviour
{
    public GameObject[] caps;
    public GameObject[] bodies;
    public GameObject[] shirts;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var cap in caps)
        {
            cap.SetActive(false);
        }

        foreach (var body in bodies)
        {
            body.SetActive(false);
        }

        foreach (var shirt in shirts)
        {
            shirt.SetActive(false);
        }

        SelectOutfit();
    }

    private void SelectOutfit()
    {
        var cap = caps[Random.Range(0, caps.Length)];
        var body = bodies[Random.Range(0, bodies.Length)];
        var shirt = shirts[Random.Range(0, shirts.Length)];

        cap.SetActive(true);
        body.SetActive(true);
        shirt.SetActive(true);
    }
}