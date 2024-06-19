using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceKnifePickup : MonoBehaviour, IPickup
{


    wallRun player;

    public bool isInRange = false;

    public KnifeStats knife;

    public bool Pickedup;



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
        if (isInRange)
        {
            knife.CurrentKinfeCount = knife.MaxKinfeCount;
            if(gameObject != null )
            {
                Destroy(gameObject);
                isInRange = false;
            }
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
