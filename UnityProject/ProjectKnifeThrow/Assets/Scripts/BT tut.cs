using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTtut : MonoBehaviour
{
    Grindtut grind;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (grind != null)
            {
                grind.CloseGrindMessage();
            }
            OpenBTMessage("");
        }
        
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseBTMessage();
        }
    }

    public void OpenBTMessage(string text)
    {
        GameManager.instance.BTMessage.SetActive(true);
    }

    public void CloseBTMessage()
    {
        GameManager.instance.BTMessage.SetActive(false);
    }
}
