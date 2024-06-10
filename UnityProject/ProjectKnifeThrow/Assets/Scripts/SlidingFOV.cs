// Ignore Spelling: Fov

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFovController : MonoBehaviour
{
    [SerializeField] float fovChangeDuration = 0.5f;
    [SerializeField] float normalFov = 60f;
    [SerializeField] float slideFov = 70f;

    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null )
        {
            // Debug.LogError("PlayerFovController: No component found on the GameObject");
        }
    }

    public void IncreaseFovForSlide()
    {
        StartCoroutine(LerpFov(playerCamera.fieldOfView, slideFov, fovChangeDuration));
    }

    public void ResetFov()
    {
        StartCoroutine(LerpFov(playerCamera.fieldOfView, normalFov, fovChangeDuration));
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
}