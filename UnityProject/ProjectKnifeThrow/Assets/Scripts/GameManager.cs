using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Menus")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText;

    [Header("HUD Components")]
    public Image playerHPBar;
    public Image playerBTBar;
    public bool isPaused;
    public int enemyCount;
    public GameObject playerBPUI;
    public GameObject playerFlashDamage;

    [Header("Object Access")]
    public GameObject player;
    public wallRun playerScript;
    public kwallRun kplayerCS;
    public KnifeSpawnners spawnners;
    public enemyAI AIScript;
    public GrindScript grindScript;
    public bool doorIsDestroyable = false;
    [SerializeField] GameObject messagePanel;
    public GameObject keyRejection;
    public GameObject keyAcceptance;
    public KeyTurnIn keys;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<wallRun>();
        grindScript = player.GetComponent<GrindScript>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }
    }

    public void FindPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnPause()
    {
        isPaused = !isPaused;
        // Risky to do 1, use unity time scale inside unity
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        // F0 is how many numbers after a decimal place
        enemyCountText.text = enemyCount.ToString("F0");

        if (enemyCount == 2 && doorIsDestroyable)
        {
            Destroy(GameObject.FindWithTag("Door"));            
        }
        if (enemyCount <= 0) 
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void spawnEnemy()
    {

    }

    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel(string text)
    {
        messagePanel.SetActive(false);
    }
    public void OpenRejectPanel(string text)
    {
        keyRejection.SetActive(true);
    }

    public void CloseRejectPanel(string text)
    {
        keyRejection.SetActive(false);
    }

    public void OpenAcceptPanel(string text)
    {
        keyAcceptance.SetActive(true);
    }

    public void CloseAcceptPanel(string text)
    {
        keyAcceptance.SetActive(false);
    }
}