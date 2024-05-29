// Ignore Spelling: Fov

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFovController : MonoBehaviour
{
    [SerializeField] float defaultFov = 60f;
    [SerializeField] float slideFovIncrease = 20f;
    [SerializeField] float fovChangeDuration = 0.5f;

    Coroutine fovChangeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.fieldOfView = defaultFov;
    }

    public void IncreaseFovForSlide()
    {
        // If there's already a FOV change in progress, stop it
        if (fovChangeCoroutine != null)
        {
            StopCoroutine(fovChangeCoroutine);
        }
        // Start a new coroutine to gradually increase FOV
        fovChangeCoroutine = StartCoroutine(ChangeFov(defaultFov + slideFovIncrease));
    }

    public void ResetFov()
    {
        // If there's already a FOV change in progress, stop it
        if (fovChangeCoroutine != null)
        {
            StopCoroutine(fovChangeCoroutine);
        }
        // Start a new coroutine to gradually reset FOV to default value
        fovChangeCoroutine = StartCoroutine(ChangeFov(defaultFov));
    }

    IEnumerator ChangeFov(float targetFov)
    {
        float initialFov = Camera.main.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < fovChangeDuration)
        {
            // Gradually change FOV over time
            Camera.main.fieldOfView = Mathf.Lerp(initialFov, targetFov, elapsedTime / fovChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = targetFov;

        fovChangeCoroutine = null;
    }
}