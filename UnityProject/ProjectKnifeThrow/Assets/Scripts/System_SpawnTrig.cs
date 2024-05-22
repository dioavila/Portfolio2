using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrig : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] int numberToSpawn;
    [SerializeField] List<Transform> spawnLocationList = new List<Transform>();
     bool isSpawning = false, spawnStart = false;
    int numberSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(numberToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnStart && !isSpawning && numberSpawned < numberToSpawn)
        {
                StartCoroutine(Spawn());   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnStart = true;
        }
    }

    IEnumerator Spawn()
    {
        isSpawning = true;
        for(int spawnLocIter = 0; spawnLocIter < spawnLocationList.Count; ++spawnLocIter)
        {
            Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
        }
        numberSpawned++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
}
