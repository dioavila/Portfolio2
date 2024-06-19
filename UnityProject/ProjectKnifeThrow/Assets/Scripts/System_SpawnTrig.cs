using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrig : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool masterTrigger = true;
    [SerializeField] bool goal = false;

    [Header("Door Settings")]
    [SerializeField] bool triggersDoor = true;
    [SerializeField] GameObject doorsToOpen;
    [SerializeField] int spawnTimer;

    [Header("Enemy Settings")]
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] List<Transform> spawnLocationList = new List<Transform>();

    [Header("Camera Shake Settings")]
    [SerializeField] float time;
    [SerializeField] float strength;
    
    bool isSpawning = false, spawnStart = false, spawnDone = false;
    int numberSpawned = 0;
    public int enemiesAlive = 0;
    CameraShake Shake;


    // Start is called before the first frame update
    void Start()
    {
        if (goal)
        {
            GameManager.instance.updateGameGoal(numberToSpawn);
        }
            Shake = FindAnyObjectByType<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnStart && !isSpawning && numberSpawned < numberToSpawn)
        {
            StartCoroutine(Spawn());
        }
        else if (spawnStart && !isSpawning && numberSpawned >= numberToSpawn)
        {
            spawnDone = true;
        }
        if (masterTrigger)
        {
            if (spawnDone && enemiesAlive <= 0 && spawnStart)
            {
                if (triggersDoor)
                {
                    doorsToOpen.GetComponent<DoorControl>().clearToOpen = true;
                }
            }
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
            if (enemyToSpawn.name == "Enemy - Destroyer (DB) v2.0.0 1")
            {
                Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
                StartCoroutine(Shake.Shake(time, strength));
            }
            else
            {
                Instantiate(enemyToSpawn, spawnLocationList[spawnLocIter].position, spawnLocationList[spawnLocIter].rotation);
            }
            if (masterTrigger)
            {
                enemiesAlive++;
            }
            else
            {
                GameManager.instance.sceneSpawners[GameManager.instance.sceneBattleRoomIndex].GetComponent<SpawnTrig>().enemiesAlive++;
            }
        }
        numberSpawned++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
}
