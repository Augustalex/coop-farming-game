using Cinemachine;
using TMPro;
using UnityEngine;

public class ItemPriceOverheadText : MonoBehaviour
{
    private TMP_Text _text;
    private CinemachineVirtualCamera _camera;
    private PlayerItem _playerItem;
    private bool _showing;
    private string _textContent;

    void Start()
    {
        _camera = GameManager.Instance.mainVirtualCamera;
        _text = GetComponentInChildren<TMP_Text>();
        _playerItem = GetComponentInParent<PlayerItem>();

        _playerItem.Grabbed += OnGrabbed;
        
        _playerItem.Dropped += OnDropped;
        _playerItem.Escaped += OnDropped;
    }

    private void OnDropped()
    {
        _showing = false;
    }

    private void OnGrabbed()
    {
        _showing = true;
    }

    void Update()
    {
        var current = transform.rotation;
        transform.LookAt(_camera.transform);
        var newRotation = transform.rotation;
        transform.rotation = new Quaternion(
            current.x,
            newRotation.y,
            current.z,
            newRotation.w
        );

        if (!_showing)
        {
            _text.text = "";
        }
        else
        {
            _text.text = _textContent;
        }
    }

    public void SetText(string text)
    {
        _textContent = text;
    }
}