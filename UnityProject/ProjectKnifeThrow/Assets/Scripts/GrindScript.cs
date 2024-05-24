using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class GrindScript : MonoBehaviour
{
    [Header("Grind Detection")]
    bool canGrind = false;
    public List<Transform> grindPoints;

    [Header("Grind Settings")]
    [SerializeField] [Range(0, 1)] float startPosLerpRate = 0.75f;
    [SerializeField] [Range(0, 1)] float grindLerpRate = 0.5f;
    [SerializeField] float speedMod;

    Vector3 p0, p1, p2, p3;

    // Update is called once per frame
    void Update()
    {
        GrindCheck();

        if(Input.GetKeyDown(KeyCode.E) && canGrind)
        {
            StartGrind();
        }
    }

    void GrindCheck()
    {
        if (grindPoints.Count == 4)
        {
            if (grindPoints[0].GetComponentInParent<GrindPointsLogic>().inRangePlayer && grindPoints[0] != null)
            {
                canGrind = true;
            }
            else
            {
                if (canGrind)
                {
                    canGrind = false;
                }
            }
        }
    }

    void StartGrind()
    {
        createCirclePath();
        GameManager.instance.playerScript.isGrinding = true;
        GameManager.instance.playerScript.playerCanMove = false;
        transform.position = Vector3.Lerp(transform.position, p0, startPosLerpRate);
        StartCoroutine(grindAction());
    }

    void createCirclePath()
    {
        //Set up start and endpoints
        p0 = grindPoints[0].position;
        p1 = grindPoints[1].position;
        p2 = grindPoints[2].position;
        p3 = grindPoints[3].position;
    }

    IEnumerator grindAction()
    {
        float timerVar = 0;

        //Grind Period
        while (timerVar < 1)
        {
            timerVar += Time.deltaTime * speedMod;
            Vector3 newPoint = ReturnPoint(timerVar);
            transform.position = Vector3.Lerp(transform.position, newPoint, grindLerpRate);
            if (GameManager.instance.playerScript.controller.isGrounded || grindPoints.Count == 0)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        //End of Grind
        canGrind = false;
        GameManager.instance.playerScript.playerCanMove = true;
        GameManager.instance.playerScript.isGrinding = false;
    }

    Vector3 ReturnPoint(float timerSpot)
    {
        //Bezier Curve Equation
        float3 pointC = Mathf.Pow(1 - timerSpot, 3) * p0 + 
            3 * Mathf.Pow(1 - timerSpot, 2) * timerSpot * p1 +
            3 * (1 - timerSpot) * Mathf.Pow(timerSpot, 2) * p2 +
            Mathf.Pow(timerSpot, 3) * p3;
        return pointC;
    }
}
