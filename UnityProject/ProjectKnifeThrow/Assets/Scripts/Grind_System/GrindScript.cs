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
    [SerializeField] [Range(0, 100)] int grindLerpRate = 1;
    [SerializeField] float speedMod;
    int pointNumber;

    // Update is called once per frame
    void Update()
    {
        ClearList();
        GrindCheck();
        

        if(Input.GetKeyDown(KeyCode.E) && canGrind && !GameManager.instance.playerScript.isGrinding && !GameManager.instance.playerScript.recoverOn)
        {

            StartGrind();

        }
    }

    void ClearList()
    {
        if (destroyedCount > 0)
        {
            grindPoints.Clear();
            destroyedCount = 0;
        }
    }

    void GrindCheck()
    {
        if (grindPoints.Count > 1)
        {
            pointNumber = grindPoints.Count;
            CreateLineConnection();
            if (grindPoints[grindPoints.Count - 1] != null && grindPoints[grindPoints.Count - 1].GetComponentInParent<GrindPointsLogic>().inRangePlayer) // && grindPoints[grindPoints.Count - 1] != null)
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
        GameManager.instance.playerScript.isGrinding = true;
        GameManager.instance.playerScript.playerCanMove = false;
        GameManager.instance.playerScript.anim.SetBool("grindOn", true);
        transform.position = Vector3.Lerp(transform.position, grindPoints[grindPoints.Count - 1].position, startPosLerpRate * Time.deltaTime);
        if (grindPoints.Count != 0)
        {
            StartCoroutine(grindAction());
        }
    }

    IEnumerator grindAction()
    {
        float timerVar = 0;
        Vector3 newPoint;
        //Grind Period
        while (timerVar < 1)
        {
            timerVar += Time.deltaTime * speedMod;
            if (pointNumber == 2)
            {
                newPoint = ReturnPointLinear(timerVar);
                
            }
            else if (pointNumber == 3)
            {
                newPoint = ReturnPointQuadratic(timerVar);
            }
            else 
            {
                newPoint = ReturnPointCubic(timerVar);
            }
            //transform.position = Vector3.Lerp(transform.position, newPoint, grindLerpRate);
            Vector3 mov = newPoint - transform.position;
            GameManager.instance.playerScript.controller.Move(mov * Time.deltaTime * grindLerpRate);

            if (GameManager.instance.playerScript.controller.isGrounded || GameManager.instance.playerScript.controller.collisionFlags == CollisionFlags.Sides || Input.GetButtonDown("Jump"))
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        //End of Grind
        canGrind = false;
        GameManager.instance.playerScript.playerCanMove = true;
        GameManager.instance.playerScript.isGrinding = false;
        GameManager.instance.playerScript.anim.SetBool("grindOn", false);
        GameManager.instance.playerScript.jump();
    }

    Vector3 ReturnPointCubic(float timerSpot)
    {
        //Bezier Curve Equation Quadratic
        float3 pointC = Mathf.Pow(1 - timerSpot, 3) * grindPoints[3].position + 
            3 * Mathf.Pow(1 - timerSpot, 2) * timerSpot * grindPoints[2].position +
            3 * (1 - timerSpot) * Mathf.Pow(timerSpot, 2) * grindPoints[1].position +
            Mathf.Pow(timerSpot, 3) * grindPoints[0].position;
        return pointC;
    }

    Vector3 ReturnPointLinear(float timerSpot)
    {
        //Bezier Curve Equation Quadratic
        float3 pointL = grindPoints[1].position +
            timerSpot * (grindPoints[0].position - grindPoints[1].position);
        return pointL;
    }
    Vector3 ReturnPointQuadratic(float timerSpot)
    {
        //Bezier Curve Equation Quadratic
        float3 pointQ = Mathf.Pow(1 - timerSpot, 2) * grindPoints[2].position +
            2 * (1 - timerSpot)*timerSpot*grindPoints[1].position +
            Mathf.Pow(timerSpot, 2) * grindPoints[0].position;
        return pointQ;
    }

    //CreateLine
    void CreateLineConnection()
    {
        if (grindPoints[0] != null)
        {
            LineRenderer line = grindPoints[0].GetComponent<LineRenderer>();
            line.positionCount = grindPoints.Count;
            for (int vertexIndex = 0; vertexIndex < line.positionCount; ++vertexIndex)
            {
                line.SetPosition(vertexIndex, grindPoints[vertexIndex].position);
            }
        }
    }
}
