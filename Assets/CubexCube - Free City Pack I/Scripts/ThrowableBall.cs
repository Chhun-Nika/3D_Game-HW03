using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableBall : MonoBehaviour
{
    [Header("Physics (editable)")]
    public float mass = 1f;
    public float drag = 0.0f;
    public float angularDrag = 0.05f;

    [Header("Bounce / Stick")]
    public float bounceForceOnWall = 0f;    // if non-zero and not matching color, add an impulse
    [Tooltip("Small offset so stuck ball doesn't clip into the wall")]
    public float stickOffset = 0.01f;

    // internal state
    private Rigidbody rb;
    private Renderer rend;
    [HideInInspector] public bool IsHeld = false;
    private bool isStuck = false;
    private Transform stuckTo = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        // apply Inspector physics values
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angularDrag;
    }

    // Called by PlayerThrowBall when picking up
    public void OnPickup(Transform holdPos)
    {
        IsHeld = true;
        isStuck = false;
        stuckTo = null;

        rb.isKinematic = true;            // disable physics while held
        transform.SetParent(holdPos);
        transform.position = holdPos.position;
        transform.rotation = holdPos.rotation;
    }

    // Called by PlayerThrowBall when dropping
    public void OnDrop()
    {
        IsHeld = false;
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    // Called by PlayerThrowBall when throwing
    public void OnThrow(Vector3 force, float colorMatchTolerance)
    {
        IsHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;  // Enable physics so it can move

        rb.velocity = Vector3.zero; // clear previous motion
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(force, ForceMode.Impulse); // Apply throw
        lastThrowColorTolerance = colorMatchTolerance;
    }


    // store last throw tolerance for next collision
    private float lastThrowColorTolerance = 0.05f;

    void OnCollisionEnter(Collision collision)
    {
        // if already stuck or currently held, ignore
        if (IsHeld || isStuck) return;

        // only consider collisions with objects tagged "Wall"
        if (!collision.gameObject.CompareTag("Wall")) return;

        // get ball color and wall color
        Color ballColor = GetMaterialColorSafe();
        Color wallColor = GetMaterialColorFromRenderer(collision.gameObject);

        // compare colors
        bool match = ColorsApproximatelyEqual(ballColor, wallColor, lastThrowColorTolerance);

        if (match)
        {
            // STICK: make kinematic and parent to wall at contact point
            ContactPoint c = collision.GetContact(0);
            Vector3 contactPos = c.point;
            Vector3 contactNormal = c.normal;

            // place ball slightly off the wall so it doesn't clip
            transform.position = contactPos + contactNormal * stickOffset;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            transform.SetParent(collision.transform, true);
            isStuck = true;
            stuckTo = collision.transform;

            // optional: call an event or play sound
            Debug.Log($"Ball stuck to wall ({collision.gameObject.name}) - color match!");
        }
        else
        {
            // NOT matching: optionally add bounce impulse or let Unity physics handle it
            if (bounceForceOnWall != 0f)
            {
                // bounce away along contact normal
                ContactPoint c = collision.GetContact(-5);
                rb.AddForce(c.normal * bounceForceOnWall, ForceMode.Impulse);
            }

            // Debug message
            Debug.Log("Ball hit wall but colors didn't match — not sticking.");
        }
    }

    Color GetMaterialColorSafe()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            return rend.material.color;
        }
        return Color.clear;
    }

    Color GetMaterialColorFromRenderer(GameObject go)
    {
        Renderer r = go.GetComponent<Renderer>();
        if (r != null && r.material != null) return r.material.color;
        return Color.clear;
    }

    bool ColorsApproximatelyEqual(Color a, Color b, float tolerance)
    {
        // compare RGB distances (ignore alpha)
        float dr = a.r - b.r;
        float dg = a.g - b.g;
        float db = a.b - b.b;
        float dist = Mathf.Sqrt(dr * dr + dg * dg + db * db);
        return dist <= Mathf.Clamp01(tolerance);
    }

    // Optional helper: if wall moves and ball stuck, keep it parented so it moves with wall.
}
