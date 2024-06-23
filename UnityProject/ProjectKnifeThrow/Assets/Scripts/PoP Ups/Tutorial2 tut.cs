using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2Tut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenTutorial2Message("");
            Invoke("CloseTutorial2Message", 5);
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
