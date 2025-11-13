using UnityEngine;

public class PickableBall : MonoBehaviour
{
    public Color ballColor;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = ballColor;
    }
}
