using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashtut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDashMessage("");
            Invoke("CloseDashMessage", 10);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseDashMessage();
        }
    }

    public void OpenDashMessage(string text)
    {
        GameManager.instance.dashMes.SetActive(true);
    }

    public void CloseDashMessage()
    {
        GameManager.instance.dashMes.SetActive(false);
    }
}
