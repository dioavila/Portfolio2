using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] public GameObject DisapperingFloor;
    [SerializeField] Rigidbody rb;
    [SerializeField] int time;
    Renderer model;
    GameObject Floor;
    bool Active;
    // Start is called before the first frame update
    void Start()
    {
        model = DisapperingFloor.GetComponent<Renderer>();
        Floor = model.gameObject;
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
        }
    }
    
    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(time * 2);
        Instantiate(DisapperingFloor);
        Active = true;
    }

    IEnumerator Disappear()
    {
        for (int i = 0; i < time; i++)
        {
            Active = false;
            model.material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            model.material.color = Color.white;
            yield return new WaitForSeconds(1f);
        }
        DisapperingFloor.SetActive(false);
    }
}
