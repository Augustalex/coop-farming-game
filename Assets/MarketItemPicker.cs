using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class MarketItemPicker : MonoBehaviour, IUIController
{
    public GameObject spawnLocation;
    public GameObject cursor;

    public event Action Submitted;

    private List<ShopItem> _shopItems;
    private int _index;
    private float _moveFreeze;
    private CursorAnimation _cursor;

    private void Awake()
    {
        DeactivateUI();

        _cursor = cursor.GetComponent<CursorAnimation>();
    }

    void Update()
    {
        if (cursor.activeSelf)
        {
            _cursor.SetTarget(_shopItems[_index].transform);
        }
    }

    public void ActivateUI()
    {
        ReloadPapers();
        cursor.SetActive(true);
    }

    public void DeactivateUI()
    {
        cursor.SetActive(false);
    }

    public void Move(Vector3 move)
    {
        if (Time.time < _moveFreeze) return;

        var startIndex = _index;

        if (move.x > 0) _index += 1;
        if (move.x < 0) _index -= 1;

        if (_index >= _shopItems.Count) _index = startIndex;
        if (_index < 0) _index = startIndex;

        if (_index != startIndex)
        {
            Sounds.Instance.PlayMoveCursorSound(_shopItems[_index].transform.position);
            _moveFreeze = Time.time + .4f;
        }
    }

    public void PlayerSubmit()
    {
        var shopItem = _shopItems[_index];

        var gameManager = GameManager.Instance;
        if (gameManager.money < shopItem.cost) return;

        _shopItems.RemoveAt(_index);
        _index = 0;

        gameManager.UseMoney(shopItem.cost);
        var item = Instantiate(shopItem.itemTemplate);
        item.transform.position = spawnLocation.transform.position;
        item.transform.rotation = spawnLocation.transform.rotation;

        Submitted?.Invoke();

        Sounds.Instance.PlayBuyItemSound(gameManager.GetCameraPosition());
    }

    private void ReloadPapers()
    {
        _index = 0;
        _shopItems = GetComponentsInChildren<ShopItem>().ToList();
    }

    public void MoveReset()
    {
        if (_moveFreeze > Time.time + .1f) _moveFreeze = Time.time + .1f;
    }
}