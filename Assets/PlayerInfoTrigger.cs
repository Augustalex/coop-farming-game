using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerInfoTarget"))
        {
            var target = other.GetComponent<PlayerInfoTriggerTarget>();
            target.Trigger();
        }
    }
}
