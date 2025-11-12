using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;

    void Update()
    {
        // Rotate around Z axis continuously
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}

