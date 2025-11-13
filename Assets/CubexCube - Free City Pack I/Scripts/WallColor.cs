using UnityEngine;

public class WallColor : MonoBehaviour
{
    public Color wallColor;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            wallColor = rend.material.color;
    }
}
