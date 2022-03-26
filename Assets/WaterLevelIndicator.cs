using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WaterLevelIndicator : MonoBehaviour
{
    public GameObject largeIndicatorTemplate;
    public GameObject smallIndicatorTemplate;

    public enum WaterLevelIndicatorState
    {
        Dry,
        Wet
    }

    public enum WaterLevelIndicatorSize
    {
        Small,
        Large
    }

    public WaterLevelIndicatorState state = WaterLevelIndicatorState.Wet;
    public WaterLevelIndicatorSize size = WaterLevelIndicatorSize.Large;


    public int level;

    public Material empty;
    public Material water;
    public Material dry;

    public Transform leftAlignedPivot;

    private int _indicatorCount;
    private readonly List<GameObject> _activeIndicators = new List<GameObject>();

    public void OnChange(int newLevel, int levelDataMaxWaterLevels, WaterLevelIndicatorState newState,
        WaterLevelIndicatorSize newSize)
    {
        _indicatorCount = levelDataMaxWaterLevels;
        level = newLevel;
        state = newState;
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

        for (int i = 0; i < _indicatorCount; i++)
        {
            var position = leftAlignedPivot.position + GetOffset(i);
            var indicator = Instantiate(
                size == WaterLevelIndicatorSize.Large ? largeIndicatorTemplate : smallIndicatorTemplate,
                position,
                Quaternion.identity,
                transform
            );

            var material = empty;
            if ((i + 1) <= level)
            {
                if (state == WaterLevelIndicatorState.Wet)
                {
                    material = water;
                }
                else if (state == WaterLevelIndicatorState.Dry)
                {
                    material = dry;
                }
            }

            indicator.GetComponentInChildren<MeshRenderer>().materials =
                new[] {material};

            _activeIndicators.Add(indicator);
        }
    }

    private Vector3 GetOffset(int index)
    {
        var indicatorSize = size == WaterLevelIndicatorSize.Large
            ? largeIndicatorTemplate.GetComponentInChildren<IndicatorCube>().transform.localScale.x
            : smallIndicatorTemplate.GetComponentInChildren<IndicatorCube>().transform.localScale.x;
        var margin = .025f;

        return (Vector3.right * (indicatorSize + margin)) * index;
    }
}