using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grindtut : MonoBehaviour
{
    private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            OpenGrindMessage("");
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            CloseGrindMessage();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseGrindMessage();
        }
    }

    public void OpenGrindMessage(string text)
    {
        GameManager.instance.grindMes.SetActive(true);
    }

    public void CloseGrindMessage()
    {
        GameManager.instance.grindMes.SetActive(false);
    }
}
