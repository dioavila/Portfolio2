using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Vector3 Position;
    public bool Active;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

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
