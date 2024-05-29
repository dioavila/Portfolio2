using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    [SerializeField] Renderer model;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            GameManager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(displayPopup());
        }
    }

    IEnumerator displayPopup()
    {
        model.material.color = Color.green;
        GameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.checkpointPopup.SetActive(false);
        model.material.color = Color.blue;
    }
}
