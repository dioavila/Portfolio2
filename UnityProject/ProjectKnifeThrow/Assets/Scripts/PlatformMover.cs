using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    // Enum to select the direction of movement
    public enum MovementDirection { Horizontal, Vertical }
    public MovementDirection direction = MovementDirection.Horizontal;


    public float speed = 2.0f;
    public float distance = 3.0f;


    private Vector3 startPosition;
    private bool movingPositive = true;
    private Rigidbody rb;

    void Start()
    {
        //initial position of the platform
        startPosition = transform.position;

        // Get the Rigidbody component 
        rb = GetComponent<Rigidbody>();

        // Make sure the Rigidbody is kinematic
        rb.isKinematic = true;
    }

    void Update()
    {
        // Determine the direction vector based on the selected movement direction
        Vector3 directionVector = direction == MovementDirection.Horizontal ? Vector3.right : Vector3.up;

        // Calculate the target position based on the current direction
        float maxOffset = movingPositive ? distance : -distance;
        Vector3 targetPosition = startPosition + directionVector * maxOffset;

        // Move the platform towards the position
        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime));

        // Check if the platform has reached the position
        if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
        {
            // Reverse the movement
            movingPositive = !movingPositive;
        }
    }
}