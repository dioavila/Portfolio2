using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunTut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenWallRunMessage("");
            Invoke("CloseWallRunMessage", 8);
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
