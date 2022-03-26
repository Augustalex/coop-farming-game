using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class RockStockZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var rockPile = other.GetComponent<RockPile>();
            if (rockPile)
            {
                rockPile.Replenish();
                
                Sounds.Instance.PlayPlaceTileSound(other.transform.position);
            }
        }
    }
}
