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
    public int destroyedCount = 0;

    [Header("Grind Settings")]
    [SerializeField] [Range(0, 1)] float startPosLerpRate = 0.75f;
    //[SerializeField] [Range(0, 1)] float grindLerpRate = 0.5f;
    [SerializeField] float speedMod;

    // Update is called once per frame
    void Update()
    {
        ClearList();
        GrindCheck();
        

        if(Input.GetKeyDown(KeyCode.E) && canGrind)
        {

            StartGrind();

        }
    }

    void ClearList()
    {
        if (destroyedCount >= 4)
        {
            grindPoints.Clear();
            destroyedCount = 0;
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
        //createCirclePath();
        GameManager.instance.playerScript.isGrinding = true;
        GameManager.instance.playerScript.playerCanMove = false;
        transform.position = Vector3.Lerp(transform.position, grindPoints[0].position, startPosLerpRate);
        StartCoroutine(grindAction());
    }

    IEnumerator grindAction()
    {
        float timerVar = 0;

        //Grind Period
        while (timerVar < 1)
        {
            timerVar += Time.deltaTime * speedMod;
            Vector3 newPoint = ReturnPoint(timerVar);
            //transform.position = Vector3.Lerp(transform.position, newPoint, grindLerpRate);
            Vector3 mov = newPoint - transform.position;
            GameManager.instance.playerScript.controller.Move(mov * Time.deltaTime * 100);

            if (GameManager.instance.playerScript.controller.isGrounded || GameManager.instance.playerScript.controller.collisionFlags == CollisionFlags.Sides)
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
        float3 pointC = Mathf.Pow(1 - timerSpot, 3) * grindPoints[0].position + 
            3 * Mathf.Pow(1 - timerSpot, 2) * timerSpot * grindPoints[1].position +
            3 * (1 - timerSpot) * Mathf.Pow(timerSpot, 2) * grindPoints[2].position +
            Mathf.Pow(timerSpot, 3) * grindPoints[3].position;
        return pointC;
    }
}
