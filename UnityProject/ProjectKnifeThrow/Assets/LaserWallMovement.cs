using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWallMovement : MonoBehaviour
{
    [SerializeField] Transform readyPoint, endPoint;
    [SerializeField] float movSpeed = 2f;
    [SerializeField] float timerToStart = 2f;
    bool ready = false;
    Vector3 lerpDistance1, lerpDistance2;
    void Start()
    {
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
            if(lerpDistance1.magnitude <= 10)
            {
                StartCoroutine(WaitTime());
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, endPoint.position, movSpeed * Time.deltaTime);
            Debug.Log(lerpDistance2.magnitude);
            if (lerpDistance2.magnitude <= 10)
            {
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
