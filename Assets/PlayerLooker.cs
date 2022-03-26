using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OrientWith(Vector3 move)
    {
        var currentRotation = transform.rotation.eulerAngles;
        var target = transform.position + move;
        transform.LookAt(target);
        var newRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            currentRotation.x,
            newRotation.y,
            currentRotation.z
        );
    }
}
