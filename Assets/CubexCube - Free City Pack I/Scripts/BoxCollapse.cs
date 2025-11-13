using UnityEngine;

public enum CollapseStyle { FallForward, FallBackward, FallLeft, FallRight }

public class BoxCollapse : MonoBehaviour
{
    public CollapseStyle collapseStyle;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // Start as static
    }

    public void CollapseBox()
    {
        rb.isKinematic = false;

        Vector3 force = Vector3.up * Random.Range(2f, 5f);
        Vector3 torque = Vector3.zero;

        switch (collapseStyle)
        {
            case CollapseStyle.FallForward:
                force += transform.forward * 3f;
                torque = new Vector3(10f, 0f, 0f);
                break;
            case CollapseStyle.FallBackward:
                force -= transform.forward * 3f;
                torque = new Vector3(-10f, 0f, 0f);
                break;
            case CollapseStyle.FallLeft:
                force -= transform.right * 3f;
                torque = new Vector3(0f, 0f, 10f);
                break;
            case CollapseStyle.FallRight:
                force += transform.right * 3f;
                torque = new Vector3(0f, 0f, -10f);
                break;
        }

        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(torque, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            CollapseBox();
        }
    }
}
