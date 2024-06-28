using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallMovement : MonoBehaviour
{
    [SerializeField] bool isStatic = true;
    [SerializeField] public Transform readyPoint, endPoint;
    [SerializeField] float movSpeed = 2f;
    [SerializeField] float timerToStart = 2f;
    Vector3 lerpDistance1, lerpDistance2;
    bool ready = false;
    [SerializeField] bool isBossMechanic;
    [SerializeField] bool canDisable = false;
    void Start()
    {
        if (isBossMechanic)
        {
            GameManager.instance.bossManager.activeLasers++;
        }
        if (canDisable)
        {
            Player_Interact_Button script = GameObject.FindWithTag("Console1").GetComponent<Player_Interact_Button>();
            script.laserSet.Add(gameObject);
        }
        //if (readyPoint != null && endPoint != null)
        //{
        //    lerpDistance1 = transform.position - readyPoint.position;
        //    lerpDistance2 = transform.position - endPoint.position;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStatic)
        {
            if (readyPoint != null && endPoint != null)
            {
                lerpDistance1 = transform.position - readyPoint.position;
                lerpDistance2 = transform.position - endPoint.position;

                if (!ready)
                {
                    transform.position = Vector3.Lerp(transform.position, readyPoint.position, movSpeed * Time.deltaTime);
                    if (lerpDistance1.magnitude <= 1)
                    {
                        StartCoroutine(WaitTime());
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, endPoint.position, movSpeed * Time.deltaTime);
                    if (lerpDistance2.magnitude <= 1)
                    {
                        if (isBossMechanic)
                        {
                            GameManager.instance.bossManager.activeLasers--;
                        }
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(timerToStart);
        ready = true;
    }

}
