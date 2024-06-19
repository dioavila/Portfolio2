// Ignore Spelling: Fov

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFovController : MonoBehaviour
{
    [SerializeField] float fovChangeDuration = 0.5f;
    [SerializeField] float normalFov = 60f;
    [SerializeField] float forwardFov = 50f;
    [SerializeField] float backwardFov = 70f;
    [SerializeField] float tiltAngle = 15f;
    [SerializeField] float tiltSmoothDuration = 0.2f; // Additional duration for smoothing the tilt
    [SerializeField] Camera bodyCamera;
    [SerializeField] Camera mainCamera;

    private Camera playerCamera;
    private Camera playerCamera2;
    private Coroutine tiltCoroutine;

    void Start()
    {
        // Get the Camera component from the child object
        playerCamera = mainCamera;
        playerCamera2 = bodyCamera;
        if (playerCamera == null)
        {
            // Uncomment the line below to show an error if the camera is not found
            // Debug.LogError("PlayerFovController: No Camera component found on the GameObject");
        }
    }

    // Methods to start the FOV change coroutines
    public void IncreaseFovForSlide()
    {
        StartCoroutine(LerpFov(playerCamera2.fieldOfView, forwardFov, fovChangeDuration));
    }

    public void DecreaseFovForBackwardSlide()
    {
        StartCoroutine(LerpFov(playerCamera2.fieldOfView, backwardFov, fovChangeDuration));
    }

    public void ResetFov()
    {
        StartCoroutine(LerpFov(bodyCamera.fieldOfView, normalFov, fovChangeDuration));
    }

    // Methods to start the camera tilt coroutines
    public void TiltCameraLeft()
    {
        StartTiltAndReset(tiltAngle);
    }

    public void TiltCameraRight()
    {
        StartTiltAndReset(-tiltAngle);
    }

    private void StartTiltAndReset(float targetAngle)
    {
        if (tiltCoroutine != null)
        {
            StopCoroutine(tiltCoroutine);
        }
        tiltCoroutine = StartCoroutine(TiltAndReset(targetAngle, fovChangeDuration + tiltSmoothDuration));
    }

    // Coroutine to smoothly change the field of view over a specified duration
    IEnumerator LerpFov(float startFov, float endFov, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            playerCamera2.fieldOfView = Mathf.Lerp(startFov, endFov, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerCamera2.fieldOfView = endFov;
    }

    // Coroutine to tilt the camera and then return it to the original position smoothly
    IEnumerator TiltAndReset(float targetAngle, float duration)
    {
        // First part: tilt to the target angle
        float startAngle = playerCamera.transform.localEulerAngles.z;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 2)
        {
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime / (duration / 2));
            playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, currentAngle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Second part: smoothly return to the original angle
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            float currentAngle = Mathf.LerpAngle(targetAngle, 0f, elapsedTime / (duration / 2));
            playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, currentAngle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final angle is exactly 0
        playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, 0f);
    }
}