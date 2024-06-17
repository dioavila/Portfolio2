using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementtut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenMovementMessage("");
            Invoke("CloseMovementMessage", 10);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseMovementMessage();
        }
    }

    public void OpenMovementMessage(string text)
    {
        GameManager.instance.movementMessage.SetActive(true);
    }

    public void CloseMovementMessage()
    {
        GameManager.instance.movementMessage.SetActive(false);
    }
}
