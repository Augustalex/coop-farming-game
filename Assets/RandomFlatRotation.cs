using UnityEngine;

public class RandomFlatRotation : MonoBehaviour
{
    void Start()
    {
        var currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(
            currentRotation.x,
            Random.value * 365,
            currentRotation.z
        );
    }
}