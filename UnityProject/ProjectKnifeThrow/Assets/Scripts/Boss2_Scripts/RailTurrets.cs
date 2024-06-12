using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class RailTurrets : MonoBehaviour
{
    [Header("Status")]
        [SerializeField] int jointHP = 10;
        GameObject player;
        int origHP;
        public bool isActive = false;
    
    [Header("Rotation")]
        [SerializeField] float viewAngle = 180;
        [SerializeField] float turretTurnRate = 1;
        Quaternion startRot;
        Quaternion currRot;
        float angleToPlayer;

    [Header("Position")]
         [SerializeField] float turretMovRate = 1;
         [SerializeField] Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        origHP = jointHP;
        player = GameManager.instance.player;
        startRot = Quaternion.LookRotation(transform.forward, -transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 100, Color.green);
            jointRotation();
            jointMovement();
        }
        else
        {
            jointRotation(startRot);
            jointMovement(startPos);
        }
    }

    private void jointRotation()
    {
        Vector3 direction = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(direction, -transform.up);
        Debug.Log(angleToPlayer);
        if (angleToPlayer < viewAngle)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turretTurnRate * Time.deltaTime);
        }
    }

    private void jointRotation(Quaternion startRot)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, startRot, turretTurnRate * Time.deltaTime);
    }

    private void jointMovement()
    {
        transform.position = Vector3.Lerp(transform.position, (new Vector3(transform.position.x, transform.position.y, player.transform.position.z)), turretMovRate * Time.deltaTime);
    }
    private void jointMovement(Transform startPos)
    {
        transform.position = Vector3.Lerp(transform.position, startPos.position, turretMovRate * Time.deltaTime);
    }
}

