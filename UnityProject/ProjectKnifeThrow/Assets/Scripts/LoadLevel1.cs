using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel1 : MonoBehaviour
{
    [SerializeField] string leveltoLoad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }
    public void LoadScene()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine(leveltoLoad));
    }
}

