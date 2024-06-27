using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTut : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            OpenTutorial1Message("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            CloseTutorial1Message();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseTutorial1Message();
        }
    }

    public void OpenTutorial1Message(string text)
    {
        GameManager.instance.Tutorial1Message.SetActive(true);
    }

    public void CloseTutorial1Message()
    {
        GameManager.instance.Tutorial1Message.SetActive(false);
    }
}
