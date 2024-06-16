using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDamage : MonoBehaviour, IDamage, IFire
{
    [SerializeField] int jointHP;
    [SerializeField] GameObject player;
    [SerializeField] public float limbTurnRate;
    Quaternion currRot;
    int origHP;
    Renderer jointModel;
    [SerializeField] GameObject body;
    float angleToPlayer;
    [SerializeField] float viewAngle;

    //enemyAITest enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        //enemyScript = gameObject.GetComponentInParent<enemyAITest>();
        origHP = jointHP;
        jointModel = gameObject.GetComponent<Renderer>();
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward *100, Color.green);
        jointMovement();
    }

    private void jointMovement()
    {
        Vector3 direction = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(direction, body.transform.forward);

        if (angleToPlayer < viewAngle)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation,  limbTurnRate *Time.deltaTime);
        }
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

    public void FireDamage(int amount, int time)
    {
        StartCoroutine(FireTime(amount, time));
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
                Destroy(gameObject);
            }
        }
    }
}

