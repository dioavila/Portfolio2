using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class RailTurretsMovement : MonoBehaviour
{
    [Header("Position")]
        [SerializeField] Vector3 startPos;
        public bool isActive = false;
        [SerializeField] float turrentMovRate = 1;    
        GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            jointMovement();
        }
        else
        {
           // jointMovement(startPos);
        }
    }

    private void jointMovement()
    {
        transform.position = Vector3.Lerp(transform.position, (new Vector3(transform.position.x, transform.position.y ,player.transform.position.z)), turrentMovRate * Time.deltaTime);
    }
    private void jointMovement(Vector3 startPos)
    {
        transform.position = Vector3.Lerp(transform.position, startPos, turrentMovRate * Time.deltaTime);
    }
}

