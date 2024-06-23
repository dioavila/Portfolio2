using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3Tut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenTutorial3Message("");
            Invoke("CloseTutorial3Message", 5);
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
