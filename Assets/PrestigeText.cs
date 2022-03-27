using TMPro;
using UnityEngine;

public class PrestigeText : MonoBehaviour
{
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        GameManager.Instance.PrestigeChanged += UpdateText;
        
        UpdateText(0);
    }

    private void UpdateText(int prestige)
    {
        _text.text = $"Prestige: {prestige}";
    }
}