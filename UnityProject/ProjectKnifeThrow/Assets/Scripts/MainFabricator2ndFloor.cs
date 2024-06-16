using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFabricator2ndFloor : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] int numberToSpawn;
    [SerializeField] List<Transform> spawnLocationList = new List<Transform>();
    [SerializeField] bool goal = false;
    bool isSpawning = false, spawnStart = false;
    int numberSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (goal)
        {
            GameManager.instance.updateGameGoal(numberToSpawn);
        }
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
        for (int spawnLocIter = 0; spawnLocIter < spawnLocationList.Count; ++spawnLocIter)
        {
            Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
        }
        numberSpawned++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
}
