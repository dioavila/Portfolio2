using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindPointsLogic : MonoBehaviour
{
    [Header("Grind Logic")]
    [SerializeField] Transform gPoint;
    public bool inRangePlayer = false;

    [Header("Destruction Settings")]
    [SerializeField] int destructionTimerMax;
    [SerializeField] [Range(0,3)] int destructionSpeed;
    float destructionTimerCurr;

    // Start is called before the first frame update
    void Start()
    {
        //Can use start to initiate VFX
        GameManager.instance.grindScript.grindPoints.Add(gPoint);
        destructionTimerCurr = destructionTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.grindScript.grindPoints.Count >= 4)
        {
            DestroyStart();
        }
    }

    void DestroyStart()
    {
        if (destructionTimerCurr > 0)
        {
            destructionTimerCurr -= Time.deltaTime * destructionSpeed;
        }
        else if (destructionTimerCurr <= 0 && !GameManager.instance.playerScript.isGrinding)
        {
            GameManager.instance.playerScript.gThrowCount--;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangePlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangePlayer = false;
        }
    }
     void OnDestroy()
    {
        //GameManager.instance.grindScript.grindPoints.Remove(gPoint);
        GameManager.instance.grindScript.destroyedCount++;
    }
}
