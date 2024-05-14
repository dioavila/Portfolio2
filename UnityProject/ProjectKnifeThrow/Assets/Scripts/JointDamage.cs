using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDamage : enemyAITest, IDamage
{
    [SerializeField] int jointHP;
    [SerializeField] Transform player;
    [SerializeField] float limbTurnRate;
    Quaternion currRot;
    int origHP;
    enemyAITest enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = gameObject.GetComponentInParent<enemyAITest>();
        origHP = jointHP;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward *100, Color.green);
      //  if (enemyScript.playerInRange)
      //  {
            jointMovement();
      //  }
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
        if ( jointHP <= 0 ) 
        {
            Destroy(gameObject);
        }
    }
}