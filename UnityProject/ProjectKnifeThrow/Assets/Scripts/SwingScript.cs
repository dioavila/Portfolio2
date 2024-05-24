using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SwingScript : MonoBehaviour
{
    [SerializeField] Transform gunPos;
    [SerializeField] Transform cam;
    [SerializeField] int grappleDist;
    [SerializeField] float playerHeight;
    [SerializeField] float lowerCirclePointsScale;
    [SerializeField] LayerMask customMask;
    [SerializeField] float speedMod;
    [SerializeField] Transform deb;
    BezierCurve swingPath;
    Vector3 p0, p1, p2, p3;
    bool swinging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) //&& GameManager.instance.playerScript.onAir)
        {
            StartSwing();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Grapple Cancel!");
            //StopSwing();
        }
    }

    void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunPos.position, Camera.main.transform.forward, out hit, grappleDist))// && swingPath != null)
        {
            if (hit.collider.CompareTag("Swingable"))
            {
                Debug.Log("Grapple Hit!");
                swinging = true;
                Vector3 semiCircleRadius = FindCenterCircle(hit);
                createCirclePath(semiCircleRadius);
                StartCoroutine(swingAction());
                swinging = false;

            }
        }
    }

    Vector3 FindCenterCircle(RaycastHit hit)
    {
        Vector3 downVec = Vector3.Cross(hit.transform.forward, hit.transform.right);
        Debug.DrawRay(hit.point, -downVec * 100, Color.magenta);
        float angle = Mathf.Sin(Vector2.Angle(hit.barycentricCoordinate, downVec));
        Debug.DrawRay(GameManager.instance.player.transform.position, GameManager.instance.player.transform.forward * (angle * hit.distance), Color.yellow);
        Vector3 resultant = GameManager.instance.player.transform.forward * (angle * hit.distance);
        resultant *= 1.25f;
        return resultant;
    }

    void createCirclePath(Vector3 radius)
    {
        //Set up start and endpoints
        p0 = GameManager.instance.player.transform.position;
        p3.x = radius.x;
        p3.y = p0.y;
        p3.z = p0.z;
        //Set up midpoints
        p1 = p0;
        p1.y -= (playerHeight * lowerCirclePointsScale);
        p2 = p3;
        p2.y -= (playerHeight * lowerCirclePointsScale);
        swingPath = new BezierCurve(p0,p1, p2,p3);

        //debug
        //Instantiate(deb, p0, transform.rotation);
        //Instantiate(deb, p1, transform.rotation);
        //Instantiate(deb, p2, transform.rotation);
        //Instantiate(deb, p3, transform.rotation);
    }

    IEnumerator swingAction()
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
