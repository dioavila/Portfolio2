using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class KenemyAI : MonoBehaviour, IDamage, IFreeze
{
    [SerializeField] GameObject bullet;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos1;
    [SerializeField] Transform shootPos2;
    [SerializeField] Transform shootPos3;
    [SerializeField] Transform shootPos4;

    [SerializeField] int HP;
    [SerializeField] float shootRate;

    public NavMeshAgent agent;
    public Transform player;

    bool isShooting;
    bool playerInRange;
   
    bool canshoot;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(1);
        canshoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (!isShooting && canshoot)
            {
                StartCoroutine(shoot());
            }
            if (model.material.color == Color.blue)
            {
                agent.isStopped = true;
                canshoot = false;
            }
            else
            {
                agent.isStopped = false;
                canshoot = true;
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
        transform.LookAt(player);
        isShooting = true;
        if (shootPos1 != null)
        {
            Instantiate(bullet, shootPos1.position, transform.rotation);
        }
        if (shootPos2 != null)
        {
            Instantiate(bullet, shootPos2.position, transform.rotation);
        }
        if (shootPos3 != null)
        {
            Instantiate(bullet, shootPos3.position, transform.rotation);
        }
        if (shootPos4 != null)
        {
            Instantiate(bullet, shootPos4.position, transform.rotation);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashred());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
            GameManager.instance.doorIsDestroyable = true;
        }
    }

    public void FreezeTime(int time)
    {
        
        StartCoroutine(FlashBlue(time));
      
    }

    IEnumerator flashred()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    //frozen enemy feedback
    IEnumerator FlashBlue(int time)
    {
        model.material.color = Color.blue;      
        yield return new WaitForSeconds(time);
        model.material.color = Color.white;
    }
}