using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var waterCan = other.GetComponent<WaterCanItem>();
            if (waterCan)
            {
                waterCan.OnWaterZone();   
            }
        }
    }
}
