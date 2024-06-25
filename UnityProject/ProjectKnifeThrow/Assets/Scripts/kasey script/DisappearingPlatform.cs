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
    bool Active = true;
    // Start is called before the first frame update
    void Start()
    {
        model = DisapperingFloor.GetComponent<Renderer>();
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
            Color temp = model.material.color;
            model.material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            model.material.color = temp;
            yield return new WaitForSeconds(1f);
        }
        DisapperingFloor.SetActive(false);
        Active = false;
    }
}
