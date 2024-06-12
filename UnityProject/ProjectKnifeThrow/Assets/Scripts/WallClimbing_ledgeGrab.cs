using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public CharacterController controller;

    [Header("Climbing Settings")]
    public float climbSpeed = 5f;
    public float maxClimbTime = 5f;
    public LayerMask climbableLayer;

    [Header("Climb Jump Settings")]
    public float climbJumpUpForce = 5f;
    public float climbJumpBackForce = 5f;
    public KeyCode jumpKey = KeyCode.Space;
    public int maxClimbJumps = 1;

    [Header("Detection")]
    public float detectionLength = 1f;
    public float sphereCastRadius = 0.5f;
    public float maxWallLookAngle = 60f;

    private float climbTimer;
    private bool isClimbing;
    private int climbJumpsLeft;
    private RaycastHit frontWallHit;
    private bool wallInFront;
    private float wallLookAngle;

    private void Update()
    {
        WallCheck();
        ClimbingStateMachine();

        if (isClimbing) ClimbingMovement();
    }

    private void WallCheck()
    {
        wallInFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, climbableLayer);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (wallInFront && wallLookAngle < maxWallLookAngle)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = maxClimbJumps;
        }
    }

    private void ClimbingStateMachine()
    {
        if (wallInFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!isClimbing && climbTimer > 0) StartClimbing();
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer <= 0) StopClimbing();
        }
        else
        {
            if (isClimbing) StopClimbing();
        }

        if (isClimbing && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJump();
    }

    private void StartClimbing()
    {
        isClimbing = true;
    }

    private void ClimbingMovement()
    {
        Vector3 climbDirection = Vector3.up * climbSpeed;
        controller.Move(climbDirection * Time.deltaTime);
    }

    private void StopClimbing()
    {
        isClimbing = false;
    }

    private void ClimbJump()
    {
        StopClimbing();

        Vector3 jumpForce = orientation.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;
        Vector3 moveDirection = new Vector3(jumpForce.x, jumpForce.y, jumpForce.z);

        controller.Move(moveDirection * Time.deltaTime);
        climbJumpsLeft--;
    }
}