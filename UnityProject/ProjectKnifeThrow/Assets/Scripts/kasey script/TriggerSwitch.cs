using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//This trigger only works with the knife throw && has to be the standard knife

public class TriggerSwitch : MonoBehaviour
{
    [Header("Movement Setup")]
    [SerializeField] GameObject obj;
    [SerializeField] Transform Position;
    [SerializeField] float extendRate = 1f;

    [Header("Timer Setup")]
    [SerializeField] float timerToRetract;
    [SerializeField] float coolDownTimer;
    [SerializeField] Transform startPos;
    Renderer Model;
    
    public bool Active;

    // Start is called before the first frame update
    void Start()
    {
        Model = gameObject.GetComponent<Renderer>();
       // startPos = obj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            Model.material.color = Color.green;
            MoveObj(Active);

        }
        if (!Active) 
        {
            Model.material.color = Color.red;
            MoveObj(Active);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Standard Knife(Clone)" && !Active)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
    }

    void MoveObj(bool activeStatus)
    {
        if(activeStatus)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(startPos.position.x, Position.position.y, startPos.position.z), Time.deltaTime * extendRate);
        }
        else
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, startPos.position, Time.deltaTime * extendRate);
        }
    }
}
