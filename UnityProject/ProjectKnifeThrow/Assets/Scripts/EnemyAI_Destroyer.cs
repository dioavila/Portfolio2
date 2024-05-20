using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAITest : MonoBehaviour//, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform[] shootPos = new Transform [4];
    [SerializeField] int HP;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    public bool playerInRange;
    public Transform playerLocation;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }

            if (shootPos[0] == null && shootPos[1] == null && shootPos[2] == null && shootPos[3] == null)
            {
                GameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        if (shootPos[0] != null)
        {
            Instantiate(bullet, shootPos[0].position, transform.rotation, shootPos[0]); 
            yield return new WaitForSeconds(0.2f);
        }
        if (shootPos[1] != null) {
            Instantiate(bullet, shootPos[1].position, transform.rotation, shootPos[1]); 
            yield return new WaitForSeconds(0.4f);
        }
        if (shootPos[2] != null)
        {
            Instantiate(bullet, shootPos[2].position, transform.rotation, shootPos[2]); 
            yield return new WaitForSeconds(0.1f);
        }
        if (shootPos[3] != null)
        {
            Instantiate(bullet, shootPos[3].position, transform.rotation, shootPos[3]); 
            yield return new WaitForSeconds(0.3f);
        }
        isShooting = false;
    }

    /*public void TakeDamage(int amount)
    {
        HP -= amount;
       // agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashred());

        if (HP <= 0)
        {
            //GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashred()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }*/
}