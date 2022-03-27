using UnityEngine;

public class StarBarrier : MonoBehaviour
{
    public int starLevel;

    public GameObject[] appear;
    public GameObject[] disappear;
    
    void Start()
    {
        GameManager.Instance.StarLevelChanged += UpdateBarrierStatus;
        
        UpdateBarrierStatus(0);
    }

    private void UpdateBarrierStatus(int newStarLevel)
    {
        if (newStarLevel < starLevel)
        {
            foreach (var obj in appear)
            {
                obj.SetActive(false);
            }
            foreach (var obj in disappear)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            foreach (var obj in appear)
            {
                obj.SetActive(true);
            }
            foreach (var obj in disappear)
            {
                obj.SetActive(false);
            }
        }
    }
}
