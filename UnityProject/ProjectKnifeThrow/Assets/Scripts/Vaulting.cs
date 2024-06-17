using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask vaultLayer; // Layer mask for detecting vaultable objects
    //[SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private float vaultDistance = 1.5f; // Maximum distance to detect vaultable objects
    [SerializeField] private float vaultHeight = 1.5f; // Maximum height to detect vaultable objects
    [SerializeField] private float vaultSpeed = 0.5f; // Duration of the vaulting process
    private bool isVaulting = false; 

    private void Update()
    {
        // Allowing vaulting only if not currently in the process of vaulting
        if (!isVaulting)
        {
            PerformVault();
        }
    }

    // Detects vaultable objects and initiates the vaulting process
    private void PerformVault()
    {
        RaycastHit firstHit;
        Vector3 rayOrigin = cam.transform.position;
        Vector3 rayDirection = cam.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * vaultDistance, Color.red, 1.0f);

        // Raycasting to detect vaultable objects in front of the player
        if (Physics.Raycast(rayOrigin, rayDirection, out firstHit, vaultDistance, vaultLayer))
        {
            Vector3 vaultPoint = firstHit.point;
            Vector3 upOffset = Vector3.up * vaultHeight;

            RaycastHit secondHit;
            // Raycasting to find a suitable landing spot above the vaultable object
            if (Physics.Raycast(vaultPoint + upOffset, Vector3.down, out secondHit, vaultHeight, vaultLayer))
            {
                Vector3 landingPosition = secondHit.point + Vector3.up * playerRadius;
                StartCoroutine(LerpVault(transform.position, landingPosition, vaultSpeed)); // Initiating movement to the landing spot
            }
            else
            {
                //Debug.Log("No suitable landing spot found.");
            }
        }
        else
        {
            //Debug.Log("No vaultable object detected.");
        }
    }

    // Moves the player smoothly from the starting position to the target position
    private IEnumerator LerpVault(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        isVaulting = true;
        float time = 0;

        while (time < duration)
        {
            // Smoothly moving the player towards the target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensuring the player reaches the exact target position
        isVaulting = false;
    }
}