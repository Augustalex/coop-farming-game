using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodSpawner : MonoBehaviour
{
    public GameObject rodSectionTemplate;
    public bool autoGrow = false;
    
    private float _growCooldown = 1f;
    private int _size;
    private Stack<RodSection> _sections = new Stack<RodSection>();
    private const int MaxSize = 4;

    void Start()
    {
        foreach (var child in GetComponentsInChildren<RodSection>())
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        if (autoGrow)
        {
            if (_size == MaxSize) return;
            
            if (_growCooldown > 0f)
            {
                _growCooldown -= Time.deltaTime;
            }
            else
            {
                _growCooldown = 5f;

                Grow();
            }
        }
    }

    public void Grow()
    {
        if (_size == MaxSize) return;
        
        var section = Instantiate(rodSectionTemplate, transform.position + Vector3.up * _size, Quaternion.identity,
            transform);
        _sections.Push(section.GetComponent<RodSection>());

        _size += 1;
    }

    public bool CanGrow()
    {
        return _size < MaxSize;
    }

    public bool CanBePicked()
    {
        return _size > 0;
    }

    public void Pick()
    {
        var section = _sections.Pop();
        section.Pick();

        _size -= 1;
    }
}