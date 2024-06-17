using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallMovement : MonoBehaviour
{
    [SerializeField] public Transform readyPoint, endPoint;
    [SerializeField] float movSpeed = 2f;
    [SerializeField] float timerToStart = 2f;
    Vector3 lerpDistance1, lerpDistance2;
    bool ready = false;
    [SerializeField] bool isBossMechanic;
    void Start()
    {
        if (isBossMechanic)
        {
            GameManager.instance.bossManager.activeLasers++;
        }
        lerpDistance1 = transform.position - readyPoint.position;
        lerpDistance2 = transform.position - endPoint.position;
    }

    // Update is called once per frame
    void Update()
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
            Debug.Log(lerpDistance2.magnitude);
            if (lerpDistance2.magnitude <= 1)
            {
                GameManager.instance.bossManager.activeLasers--;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(timerToStart);
        ready = true;
    }

}
