using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawnners : MonoBehaviour
{
    [SerializeField] GameObject objecttospawn;
    [SerializeField] int numbertospawn;
    [SerializeField] int spawntimer;
    [SerializeField] Transform[] spawnpos;

    int spawncount;
    bool isSpawnning;
    bool startspawning;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (startspawning && !isSpawnning && spawncount < numbertospawn)
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

    IEnumerator spawn()
    {
        isSpawnning = true;
        int arraypos = Random.Range(0, spawnpos.Length);
        Instantiate(objecttospawn, spawnpos[arraypos].position, spawnpos[arraypos].rotation);
        spawncount++;
        yield return new WaitForSeconds(spawntimer);
        isSpawnning = false;
    }
}

