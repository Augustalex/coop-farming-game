using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    public GameObject largeIndicatorTemplate;
    public GameObject smallIndicatorTemplate;

    public enum HealthLevelIndicatorSize
    {
        Small,
        Large
    }

    public HealthLevelIndicatorSize size = HealthLevelIndicatorSize.Large;


    public int indicatorCount;
    public int level;

    public Material empty;
    public Material filled;

    public Transform leftAlignedPivot;

    private readonly List<GameObject> _activeIndicators = new List<GameObject>();

    public void OnChange(int newLevel, HealthLevelIndicatorSize newSize)
    {
        level = newLevel;
        size = newSize;

        Setup();
    }

    private void Setup()
    {
        foreach (var activeIndicator in _activeIndicators)
        {
            Destroy(activeIndicator);
        }

        _activeIndicators.Clear();

        for (int i = 0; i < indicatorCount; i++)
        {
            var position = leftAlignedPivot.position + GetOffset(i);
            var indicator = Instantiate(
                size == HealthLevelIndicatorSize.Large ? largeIndicatorTemplate : smallIndicatorTemplate,
                position,
                Quaternion.identity,
                transform
            );

            indicator.GetComponentInChildren<MeshRenderer>().materials =
                new[] {i + 1 <= level ? filled : empty};

            _activeIndicators.Add(indicator);
        }
    }

    private Vector3 GetOffset(int index)
    {
        var indicatorSize = size == HealthLevelIndicatorSize.Large
            ? largeIndicatorTemplate.GetComponentInChildren<IndicatorCube>().transform.localScale.x
            : smallIndicatorTemplate.GetComponentInChildren<IndicatorCube>().transform.localScale.x;
        var margin = .025f;

        return (Vector3.right * (indicatorSize + margin)) * index;
    }
}