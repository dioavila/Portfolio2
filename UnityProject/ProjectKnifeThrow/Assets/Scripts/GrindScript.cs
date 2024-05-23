using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class GrindScript : MonoBehaviour
{
    [SerializeField] Transform gunPos;
    [SerializeField] Transform cam;
    [SerializeField] int grappleDist;
    [SerializeField] float playerHeight;
    [SerializeField] float lowerCirclePointsScale;
    [SerializeField] LayerMask customMask;
    [SerializeField] float speedMod;
    [SerializeField] Transform deb;

    public List<Transform> grindPoints;

    Vector3 p0, p1, p2, p3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && grindPoints.Count == 4)
        {
            Debug.Log("Grind Started");
            StartGrind();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            GameManager.instance.playerScript.swinging = false;
            Debug.Log("Grind Stopped");
        }
    }

    void StartGrind()
    {
        createCirclePath();
        GameManager.instance.playerScript.swinging = true;
        transform.position = p0;
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
        while (timerVar < 1)
        {
            timerVar += Time.deltaTime * speedMod;
            Vector3 newPoint = ReturnPoint(timerVar);
            transform.position = Vector3.Lerp(transform.position, newPoint, 0.5f);
            if (GameManager.instance.playerScript.controller.isGrounded)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    Vector3 ReturnPoint(float timerSpot)
    {
        float3 pointC = Mathf.Pow(1 - timerSpot, 3) * p0 + 
            3 * Mathf.Pow(1 - timerSpot, 2) * timerSpot * p1 +
            3 * (1 - timerSpot) * Mathf.Pow(timerSpot, 2) * p2 +
            Mathf.Pow(timerSpot, 3) * p3;
        return pointC;
    }
}
