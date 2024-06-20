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
    [SerializeField] float time;

    [Header("Timer Setup")]
    [SerializeField] float timerToRetract;
    [SerializeField] float coolDownTimer;
    [SerializeField] Transform startPos;
    Renderer Model;

    [SerializeField] bool triggerDoor = false;
    [SerializeField] GameObject door;
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
            if(gameObject.CompareTag("Timed Switch"))
            {
                StartCoroutine(PuzzleDoor());
            }
            else
            {
                MoveObj(Active);
            }

            if(triggerDoor)
            {
                door.GetComponent<DoorControl>().clearToOpen = true;
            }
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

    public void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && gameObject.CompareTag("Pressure Plate"))
        {
            Active = true;
            obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(Position.position.x, Position.position.y, Position.position.z), Time.deltaTime * extendRate);

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") && gameObject.CompareTag("Pressure Plate"))
        {
            Active = false;
            obj.transform.position = Vector3.Lerp(obj.transform.position, startPos.position, Time.deltaTime * extendRate);
        }
    }

    void MoveObj(bool activeStatus)
    {
        if(activeStatus)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(Position.position.x, Position.position.y, Position.position.z), Time.deltaTime * extendRate);
        }
        else
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, startPos.position, Time.deltaTime * extendRate);
        }
    }

    IEnumerator PuzzleDoor()
    {
        Active = true;
        obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(Position.position.x, Position.position.y, Position.position.z), Time.deltaTime * extendRate);
        yield return new WaitForSeconds(time);
        obj.transform.position = Vector3.Lerp(obj.transform.position, startPos.position, Time.deltaTime * extendRate);
        Active = false;
    }
}
