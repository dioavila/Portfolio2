using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDamage : MonoBehaviour, IDamage, IFreeze, IFire
{
    [SerializeField] int jointHP;
    [SerializeField] Transform player;
    [SerializeField] float limbTurnRate;
    Quaternion currRot;
    int origHP;
    Renderer jointModel;
    //enemyAITest enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        //enemyScript = gameObject.GetComponentInParent<enemyAITest>();
        origHP = jointHP;
        jointModel = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward *100, Color.green);
        jointMovement();
    }

    private void jointMovement()
    {

        Vector3 direction = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,  limbTurnRate *Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        jointHP -= damage;
        StartCoroutine(flashred());
        if ( jointHP <= 0 ) 
        {
            Destroy(gameObject);
        }


    }
    IEnumerator flashred()
    {
        Color temp = jointModel.material.color;
        jointModel.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        jointModel.material.color = temp;
    }
    public void FreezeTime(int time)
    {
        StartCoroutine(FlashBlue(time));
    }

    public void FireDamage(int amount, int time)
    {
        GameManager.instance.AIScript.agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(FireTime(amount, time));
    }
    IEnumerator FlashBlue(int time)
    {
        jointModel.material.color = Color.blue;
        yield return new WaitForSeconds(time);
        jointModel.material.color = Color.white;
    }
    IEnumerator FireTime(int amount, int time)
    {
        for (int i = 0; i <= time; i++)
        {
            jointHP -= amount;
            jointModel.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            jointModel.material.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            if (jointHP <= 0)
            {
                GameManager.instance.updateGameGoal(-1);
                Destroy(gameObject);
                GameManager.instance.doorIsDestroyable = true;
            }
        }
    }
}