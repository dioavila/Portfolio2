using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeSpawnners : MonoBehaviour
{
    [SerializeField] public GameObject objecttospawn;
    [SerializeField] int numbertospawn;
    [SerializeField] int spawntimer;
    [SerializeField] Transform[] spawnpos;

    // int spawncount;
    bool isSpawnning;
    bool startspawning;
    public bool KnifeGrabbed;


    // Start is called before the first frame update
    void Start()
    {
        KnifeGrabbed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startspawning && !isSpawnning && KnifeGrabbed)
        {
            StartCoroutine(spawn());
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startspawning = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startspawning = false;
            KnifeGrabbed = true;
        }
    }

    IEnumerator spawn()
    {
        if (KnifeGrabbed)
        {
            isSpawnning = true;
            int arraypos = Random.Range(0, spawnpos.Length);
            objecttospawn.SetActive(true);
            //Instantiate(objecttospawn, spawnpos[arraypos].position, spawnpos[arraypos].rotation);
            KnifeGrabbed = false;
            yield return new WaitUntil(() => KnifeGrabbed == true);
            yield return new WaitForSeconds(spawntimer);
            isSpawnning = false;
        }
    }
}

