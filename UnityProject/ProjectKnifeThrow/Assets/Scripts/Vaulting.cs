using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaulting : MonoBehaviour
{
    [SerializeField] private Camera cam; // Camera used for raycasting
    [SerializeField] private LayerMask vaultLayer; // Layer mask for detecting vaultable objects
    [SerializeField] private float playerHeight = 2f; 
    [SerializeField] private float playerRadius = 0.5f; 
    private bool isVaulting = false; //track if the player is currently vaulting

    private void Start()
    {
        // Inverting the vaultLayer to exclude it from raycasts
        vaultLayer = ~LayerMask.GetMask("VaultLayer");
    }

    private void Update()
    {
        // Allowing vaulting only if not currently in the process of vaulting
        if (!isVaulting && Input.GetKeyDown(KeyCode.Space))
        {
            PerformVault();
        }
    }

    private void PerformVault()
    {
        RaycastHit firstHit;
        // Raycasting to detect vaultable objects in front of the player
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out firstHit, 1, vaultLayer))
        {
            RaycastHit secondHit;
            // Raycasting to find a suitable landing spot above the vaultable object
            if (Physics.Raycast(firstHit.point + (cam.transform.forward * playerRadius) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out secondHit, playerHeight))
            {
                Vector3 landingPosition = secondHit.point + Vector3.up * playerHeight;
                StartCoroutine(LerpVault(transform.position, landingPosition, 0.5f)); // Initiating movement to the landing spot
            }
        }
    }

    private IEnumerator LerpVault(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        isVaulting = true; // Setting the vaulting to true
        float time = 0;

        while (time < duration)
        {
            // Smoothly moving the player towards the target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensuring the player reaches the exact target position
        isVaulting = false; // Resetting the vaulting 
    }
}