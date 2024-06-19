using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrig : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] bool triggersDoor = false;
    [SerializeField] GameObject doorsToOpen;
    
    [Header("Spawn Settings")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] int numberToSpawn;
    [SerializeField] List<Transform> spawnLocationList = new List<Transform>();
    [SerializeField] bool goal = false;
    bool isSpawning = false, spawnStart = false, numberUpdated = false;
    int numberSpawned = 0;
    public int enemiesAlive;
    bool spawnDone = false;
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
        else
        {
            spawnDone = true;
        }

        if (spawnDone && enemiesAlive <= 0)
        {
            if(triggersDoor)
            {
                doorsToOpen.GetComponent<DoorControl>().clearToOpen = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(numberUpdated != false)
            {
                GameManager.instance.sceneBattleRoomIndex++;
                numberUpdated = true;
            }
            spawnStart = true;
        }
    }

    IEnumerator Spawn()
    {
        isSpawning = true;
        for(int spawnLocIter = 0; spawnLocIter < spawnLocationList.Count; ++spawnLocIter)
        {
            Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
            enemiesAlive++;
        }
        numberSpawned++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
}
