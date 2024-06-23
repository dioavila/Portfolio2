using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4Tut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenTutorial4Message("");
            Invoke("CloseTutorial4Message", 5);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloseTutorial4Message();
        }
    }

    public void OpenTutorial4Message(string text)
    {
        GameManager.instance.Tutorial4Message.SetActive(true);
    }

    public void CloseTutorial4Message()
    {
        GameManager.instance.Tutorial4Message.SetActive(false);
    }
}
