using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4Tut : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            OpenTutorial4Message("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            CloseTutorial4Message();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseTutorial4Message();
        }
    }

    public void OpenTutorial4Message(string text)
    {
        GameManager.instance.Tutorial4Message.SetActive(true);
    }

    public void CloseTutorial4Message()
    {
        GameManager.instance.Tutorial4Message.SetActive(false);
    }
}
