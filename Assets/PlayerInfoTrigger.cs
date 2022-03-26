using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerInfoTarget"))
        {
            var target = other.GetComponent<PlayerInfoTriggerTarget>();
            target.Trigger();
        }
    }
}
