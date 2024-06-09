using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] GameObject DisapperingFloor;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform platformPOS;
    [SerializeField] int time;
    Renderer model;
    GameObject Floor;
    bool Active;
    // Start is called before the first frame update
    void Start()
    {
        model = DisapperingFloor.GetComponent<Renderer>();
        Floor = DisapperingFloor;
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
        {
            StartCoroutine(Reappear());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            StartCoroutine(Disappear());
            return;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if()
    //}

    IEnumerator Reappear()
    {
        Active = true;
        yield return new WaitForSeconds(time);
        DisapperingFloor.SetActive(true);
    }

    IEnumerator Disappear()
    {
        for (int i = 0; i < time; i++)
        {
            model.material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            model.material.color = Color.white;
            yield return new WaitForSeconds(1f);
        }
        DisapperingFloor.SetActive(false);
        Active = false;
    }
}
