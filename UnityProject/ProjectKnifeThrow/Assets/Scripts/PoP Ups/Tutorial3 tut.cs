using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3Tut : MonoBehaviour
{
    //private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerInTrigger = true;
            OpenTutorial3Message("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerInTrigger = false;
            CloseTutorial3Message();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseTutorial3Message();
        }
    }

    public void OpenTutorial3Message(string text)
    {
        GameManager.instance.Tutorial3Message.SetActive(true);
    }

    public void CloseTutorial3Message()
    {
        GameManager.instance.Tutorial3Message.SetActive(false);
    }
}
