using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject player;
    public GameObject referential;
    public wallRun playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        //playerScript = player.GetComponent<wallRun>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
