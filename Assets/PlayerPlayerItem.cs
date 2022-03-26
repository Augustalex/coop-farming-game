using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlayerItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<PlayerItem>().UseItem += OnUse;
    }

    void OnUse(Vector3 position)
    {
        // GetComponent<PlayerController>().Dropped(position);
    }
}
