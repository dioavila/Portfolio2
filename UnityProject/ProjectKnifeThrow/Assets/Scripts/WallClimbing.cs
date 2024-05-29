using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("References")]
    public Transform Camera; 
    public CharacterController Controller; 
    public LayerMask ClimbableLayer; 
    private wallRun wallRunScript; 

    [Header("Climbing Parameters")]
    public float ClimbSpeed = 5f; 
    public float MaxClimbTime = 5f; 
    private float _climbTimer; 

    private bool _isClimbing; 
    [Header("Climb Jumping")]
    public float ClimbJumpUpForce = 5f; // Upward force applied when jumping from a climbable surface
    public float ClimbJumpBackForce = 5f; // Backward force applied when jumping from a climbable surface
    public KeyCode StopClimbKey = KeyCode.E; 
    public int MaxClimbJumps = 1;
    private int _climbJumpsLeft; 

    [Header("Detection")]
    public float DetectionLength = 1f; // Length of the detection ray for detecting climbable surfaces
    public float SphereCastRadius = 0.5f; // Radius of the sphere cast for detecting climbable surfaces
    public float MaxWallLookAngle = 60f; // Maximum angle to consider a surface climbable
    private float _wallLookAngle; // Angle between the player's forward direction and the wall normal

    private RaycastHit _frontWallHit; // Information about the wall hit in front of the player
    private bool _wallInFront; 

    private void Start()
    {
        wallRunScript = GetComponent<wallRun>();
    }

    private void Update()
    {
        // Check for climbable surfaces and update climbing state
        WallCheck();
        StateMachine();

        // If the player is climbing, handle climbing movement
        if (_isClimbing)
        {
            ClimbingMovement();
        }
    }

    private void StateMachine()
    {
        // State machine to control climbing behavior

        // If a climbable surface is in front of the player and the climb conditions are met
        if (_wallInFront && Input.GetKey(KeyCode.W) && _wallLookAngle < MaxWallLookAngle)
        {
            // If not already climbing and there is time left to climb, start climbing
            if (!_isClimbing && _climbTimer > 0)
            {
                StartClimbing();
            }

            // Reduce climb timer while climbing conditions are met
            if (_climbTimer > 0)
            {
                _climbTimer -= Time.deltaTime;
            }

            // If the climb timer expires, stop climbing
            if (_climbTimer < 0)
            {
                StopClimbing();
            }
        }
        else
        {
            // If climbing conditions are not met, stop climbing
            if (_isClimbing)
            {
                StopClimbing();
            }
        }

        // If the climb jump key is pressed while climbing, execute a climb jump
        if (Input.GetKeyDown(StopClimbKey) && _isClimbing)
        {
            ClimbJump();
        }
    }

    private void WallCheck()
    {
        // Raycast to detect climbable surfaces in front of the player

        // Perform a sphere cast in the forward direction to detect climbable surfaces
        _wallInFront = Physics.SphereCast(transform.position, SphereCastRadius, Camera.forward, out _frontWallHit, DetectionLength, ClimbableLayer);

        // Calculate the angle between the player's forward direction and the wall normal
        _wallLookAngle = Vector3.Angle(Camera.forward, -_frontWallHit.normal);

        // If a climbable surface is detected and the angle is within the threshold, update climb timer and jumps
        if (_wallInFront && _wallLookAngle < MaxWallLookAngle)
        {
         
            Debug.Log("Wall Detected: " + _frontWallHit.collider.name);

            // Reset the climb timer and jumps
            _climbTimer = MaxClimbTime;
            _climbJumpsLeft = MaxClimbJumps;
        }
        else
        {
            
            Debug.Log("No Climbable Wall Detected");
        }
    }

    private void StartClimbing()
    {
        // Set flag to true
        _isClimbing = true;

        wallRunScript.isClimbing = true;

        Debug.Log("Started Climbing");
    }

    private void ClimbingMovement()
    {
        // Calculate the climb direction based on the player's orientation
        Vector3 climbDirection = Camera.up * ClimbSpeed;

        // Move the character controller in the climb direction
        Controller.Move(climbDirection * Time.deltaTime);

        Debug.Log("Climbing Movement - Move Vector: " + climbDirection);
    }

    private void StopClimbing()
    {
        _isClimbing = false;

        wallRunScript.isClimbing = false;

        Debug.Log("Stopped Climbing");
    }

    private void ClimbJump()
    {
        StopClimbing();

        Debug.Log("Jumped from Climbing Position");

        // Calculate the jump force based on the climb jump parameters
        Vector3 jumpForce = Camera.up * ClimbJumpUpForce + _frontWallHit.normal * ClimbJumpBackForce;

        // Apply the jump force to the character controller
        Controller.Move(jumpForce * Time.deltaTime);

        Debug.Log("Climb Jump - Jump Force: " + jumpForce);
    }
}