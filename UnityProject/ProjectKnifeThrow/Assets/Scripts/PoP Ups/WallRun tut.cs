using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunTut : MonoBehaviour
{
    //private bool isPlayerInTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerInTrigger = true;
            OpenWallRunMessage("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isPlayerInTrigger = false;
            CloseWallRunMessage();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseWallRunMessage();
        }
    }

    public void OpenWallRunMessage(string text)
    {
        GameManager.instance.WallRunMessage.SetActive(true);
    }

    public void CloseWallRunMessage()
    {
        GameManager.instance.WallRunMessage.SetActive(false);
    }
}
