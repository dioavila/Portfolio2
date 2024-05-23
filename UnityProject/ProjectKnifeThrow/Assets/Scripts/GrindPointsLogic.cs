using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindPointsLogic : MonoBehaviour
{
    [SerializeField] Transform gPoint;
    [SerializeField] int destructionTimer;
    public bool inRangePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //Can use start to initiate VFX
        GameManager.instance.grindScript.grindPoints.Add(gPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.grindScript.grindPoints.Count == 4)
        {
            DestroyStart();
        }
    }

    void DestroyStart()
    {
        Destroy(gameObject, destructionTimer);
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
    //private void OnDestroy()
    //{
    //    GameManager.instance.grindScript.grindPoints.Remove(gPoint);
    //}
}
