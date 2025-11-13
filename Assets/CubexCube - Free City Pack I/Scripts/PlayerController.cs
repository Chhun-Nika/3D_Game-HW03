using UnityEngine;

public class PlayerThrowBall : MonoBehaviour
{
    [Header("References")]
    public Transform holdPosition;
    public float pickupRange = 3f;
    public float throwForce = 10f;

    [Header("Key Controls")]
    public KeyCode pickKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public KeyCode throwKey = KeyCode.F;

    private ThrowableBall heldBall = null;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(pickKey))
        {
            if (heldBall == null)
                TryPickup();
        }

        if (Input.GetKeyDown(dropKey) && heldBall != null)
        {
            heldBall.OnDrop();
            heldBall = null;
        }

        if (Input.GetKeyDown(throwKey) && heldBall != null)
        {
            Vector3 throwDir = transform.forward;
            heldBall.OnThrow(throwDir * throwForce, 0.1f);
            heldBall = null;
        }
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider hit in hits)
        {
            ThrowableBall ball = hit.GetComponent<ThrowableBall>();
            if (ball != null)
            {
                heldBall = ball;
                heldBall.OnPickup(holdPosition);
                Debug.Log("Picked up ball: " + ball.name);
                return;
            }
        }
    }
}
