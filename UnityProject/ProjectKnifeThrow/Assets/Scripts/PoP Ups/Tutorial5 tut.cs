using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5Tut : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private Coroutine messageCoroutine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            OpenTutorial5Message("");
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(CloseMessageAfterDelay(10));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            CloseTutorial5Message();
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
                messageCoroutine = null;
            }
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && isPlayerInTrigger)
        {
            CloseTutorial5Message();
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
                messageCoroutine = null;
            }
        }
    }

    public void OpenTutorial5Message(string text)
    {
        GameManager.instance.Tutorial5Message.SetActive(true);
    }

    public void CloseTutorial5Message()
    {
        GameManager.instance.Tutorial5Message.SetActive(false);
    }

    private IEnumerator CloseMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isPlayerInTrigger)
        {
            CloseTutorial5Message();
        }
    }
}
