using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceKnifePickup : MonoBehaviour, IPickup
{
    wallRun player;

    public bool isInRange;

    KnifeStats knife;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpItem()
    {
        if (player != null)
        {
            knife.currentKinfeCount = maxkinfeCount;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            GameManager.instance.OpenMessagePanel("");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            GameManager.instance.CloseMessagePanel("");
        }
    }
}
