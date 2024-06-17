using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grindtut : MonoBehaviour
{
    Dashtut dash;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dash != null)
            {
                dash.CloseDashMessage();
            }
            OpenGrindMessage("");
            Invoke("CloseGrindMessage", 10);
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
