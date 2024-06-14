using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.transform.position = Vector3.Lerp(other.transform.position, new Vector3(gameObject.transform.position.x,0,0), Time.deltaTime * speed);
        }
    }
}
