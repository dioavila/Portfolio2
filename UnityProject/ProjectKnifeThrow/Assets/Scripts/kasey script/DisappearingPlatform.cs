using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] GameObject DisapperingFloor;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform platformPOS;
    [SerializeField] int time;
    Renderer model;
    private Color temp;
    bool colorChange;
    bool isRespawning;
    bool Active = true;
    // Start is called before the first frame update
    void Start()
    {
        model = DisapperingFloor.GetComponent<Renderer>();
        temp = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active)
        {
            if (colorChange)
                model.material.color = temp;
            if(isRespawning)
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            StartCoroutine(Disappear());
            return;
        }
    }

    IEnumerator Reappear()
    {
        isRespawning = false;
        yield return new WaitForSeconds(time);
        DisapperingFloor.SetActive(true);
        Active = true;
    }

    IEnumerator Disappear()
    {
        model.material.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        DisapperingFloor.SetActive(false);
        colorChange = true;
        Active = false;
        isRespawning = true;
    }
}
