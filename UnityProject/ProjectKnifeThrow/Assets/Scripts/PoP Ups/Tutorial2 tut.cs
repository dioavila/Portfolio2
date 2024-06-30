using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2Tut : MonoBehaviour
{
    //private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerInTrigger = true;
            OpenTutorial2Message("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           //isPlayerInTrigger = false;
           CloseTutorial2Message();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseTutorial2Message();
        }
    }

    public void OpenTutorial2Message(string text)
    {
        GameManager.instance.Tutorial2Message.SetActive(true);
    }

    public void CloseTutorial2Message()
    {
        GameManager.instance.Tutorial2Message.SetActive(false);
    }
}
