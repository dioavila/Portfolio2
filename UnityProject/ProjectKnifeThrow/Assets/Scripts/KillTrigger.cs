using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    wallRun player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.playerScript; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.youLose();
        }
    }

}
