// Ignore Spelling: Fov

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFovController : MonoBehaviour
{
    [SerializeField] float fovChangeDuration = 0.5f;
    [SerializeField] float normalFov = 60f;
    [SerializeField] float forwardFov = 70f;
    [SerializeField] float backwardFov = 50f;
    [SerializeField] float tiltAngle = 15f;
    [SerializeField] float tiltSmoothDuration = 0.2f; // Additional duration for smoothing the tilt

    private Camera playerCamera;
    private Coroutine tiltCoroutine;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            // Debug.LogError("PlayerFovController: No Camera component found on the GameObject");
        }
    }

    public void IncreaseFovForSlide()
    {
        StartCoroutine(LerpFov(playerCamera.fieldOfView, forwardFov, fovChangeDuration));
    }

    public void DecreaseFovForBackwardSlide()
    {
        StartCoroutine(LerpFov(playerCamera.fieldOfView, backwardFov, fovChangeDuration));
    }

    public void ResetFov()
    {
        StartCoroutine(LerpFov(playerCamera.fieldOfView, normalFov, fovChangeDuration));
    }

    public void TiltCameraLeft()
    {
        StartTilt(tiltAngle);
    }

    public void TiltCameraRight()
    {
        StartTilt(-tiltAngle);
    }

    public void ResetTilt(float delay = 0f)
    {
        if (tiltCoroutine != null)
        {
            StopCoroutine(tiltCoroutine);
        }
        tiltCoroutine = StartCoroutine(LerpTilt(0f, fovChangeDuration + tiltSmoothDuration, delay));
    }

    private void StartTilt(float targetAngle)
    {
        if (tiltCoroutine != null)
        {
            StopCoroutine(tiltCoroutine);
        }
        tiltCoroutine = StartCoroutine(LerpTilt(targetAngle, fovChangeDuration + tiltSmoothDuration));
    }

    IEnumerator LerpFov(float startFov, float endFov, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startFov, endFov, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerCamera.fieldOfView = endFov;
    }

    IEnumerator LerpTilt(float targetAngle, float duration, float delay = 0f)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Quaternion startRotation = playerCamera.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, targetAngle);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            playerCamera.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerCamera.transform.localRotation = endRotation;
    }
}