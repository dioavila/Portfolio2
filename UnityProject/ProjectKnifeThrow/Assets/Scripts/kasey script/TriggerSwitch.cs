using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This trigger only works with the knife throw && has to be the standard knife

public class TriggerSwitch : MonoBehaviour
{
    [SerializeField] GameObject obj;

    [SerializeField] Vector3 Position;

    Renderer Model;

    public bool Active;


    // Start is called before the first frame update
    void Start()
    {
        Model = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            Model.material.color = Color.green;
        }
        if (!Active) 
        {
            Model.material.color = Color.red;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Standard Knife(Clone)" && !Active)
        {
            obj.transform.position += new Vector3(Position.x, Position.y, Position.z);
            Active = true;
        }
        else
        {
            obj.transform.position -= new Vector3(Position.x, Position.y, Position.z);
            Active = false;
        }
    }
}
