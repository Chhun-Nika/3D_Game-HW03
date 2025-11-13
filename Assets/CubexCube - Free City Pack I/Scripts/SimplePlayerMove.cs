using UnityEngine;

public class SimplePlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;  // how fast the player turns

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Movement input (you can switch axes if needed)
        if (Input.GetKey(KeyCode.UpArrow))
            moveX -= 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            moveX += 1f;
        if (Input.GetKey(KeyCode.RightArrow))
            moveZ += 1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveZ -= 1f;

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        // Move
        if (move.magnitude > 0)
        {
            // Move the player
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            // Rotate smoothly to face move direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
