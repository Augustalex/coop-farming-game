using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    private GameManager _gameManager;
    private TMP_Text _text;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _text.text = $"${_gameManager.money}";
    }
}
