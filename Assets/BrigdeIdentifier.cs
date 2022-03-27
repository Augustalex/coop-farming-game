using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrigdeIdentifier : MonoBehaviour
{
    private RiverSoil _river;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetRiver(RiverSoil river)
    {
        river.DisableBlockers();
        _river = river;
    }

    public void Demolish()
    {
        _river.EnableBlockers();
        _river = null;

        Destroy(gameObject);
    }
}