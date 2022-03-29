using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    void LateUpdate()
    {
        transform.RotateAround(transform.position, Vector3.up, 180 * Time.deltaTime);
    }
}