using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] KnifeStats Knife;

    // Start is called before the first frame update
    void Start()
    {
        //Knife.CurrentKinfeCount = Knife.MaxKinfeCount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.kplayerscript.GetKnifeStats(Knife);
            Destroy(gameObject);
        }
    }
}
