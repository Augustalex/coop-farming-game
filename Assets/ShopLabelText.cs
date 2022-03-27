using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopLabelText : MonoBehaviour
{
    void Start()
    {
        var marketStall = GetComponentInParent<MarketStall>();
        marketStall.Toggled += OnToggled;
        
        ClearText();
    }

    private void OnToggled(bool visible)
    {
        if (visible)
        {
            SetPriceText();
        }
        else
        {
            ClearText();
        }
    }

    private void ClearText()
    {
        GetComponent<TMP_Text>().text = "";
    }

    private void SetPriceText()
    {
        var shopItem = GetComponentInParent<ShopItem>();

        GetComponent<TMP_Text>().text = $"${shopItem.cost}";
    }
}