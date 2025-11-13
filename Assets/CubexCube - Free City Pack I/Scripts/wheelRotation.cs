using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FerrisWheelRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void FixedUpdate()  // ← physics-safe
    {
        transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime, Space.Self);
    }
}

