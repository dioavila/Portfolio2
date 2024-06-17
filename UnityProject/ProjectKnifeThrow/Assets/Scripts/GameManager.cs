using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Menus")]
    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] public GameObject menuSettings;
    [SerializeField] public GameObject menuLevelSelect;
    public GameObject UI;
    [SerializeField] public GameObject invertON;
    [SerializeField] public GameObject invertOFF;
    public bool isInverted = false;
    [SerializeField] public Slider sensSlider;
    [Tooltip("The Menu for when the EXIT button is clicked")]
    public GameObject exitMenu;
    public GameObject exitMenu1;
    public GameObject exitMenu2;
    public GameObject playMenu;

    [Header("PANELS")]
    [Tooltip("The UI Panel parenting all sub menus")]
    public GameObject mainCanvas;
    [Tooltip("The UI Panel that holds the CONTROLS window tab")]
    public GameObject PanelControls;
    [Tooltip("The UI Panel that holds the VIDEO window tab")]
    public GameObject PanelVideo;
    [Tooltip("The UI Panel that holds the GAME window tab")]
    public GameObject PanelGame;
    [Tooltip("The UI Panel that holds the KEY BINDINGS window tab")]
    public GameObject PanelKeyBindings;
    [Tooltip("The UI Sub-Panel under KEY BINDINGS for MOVEMENT")]
    public GameObject PanelMovement;
    [Tooltip("The UI Sub-Panel under KEY BINDINGS for COMBAT")]
    public GameObject PanelCombat;
    [Tooltip("The UI Sub-Panel under KEY BINDINGS for GENERAL")]
    public GameObject PanelGeneral;

    [Header("SETTINGS SCREEN")]
    [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
    public GameObject lineGame;
    [Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
    public GameObject lineVideo;
    [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
    public GameObject lineControls;
    [Tooltip("Highlight Image for when KEY BINDINGS Tab is selected in Settings")]
    public GameObject lineKeyBindings;
    [Tooltip("Highlight Image for when GENERAL Sub-Tab is selected in KEY BINDINGS")]
    public GameObject lineGeneral;

    [Header("HUD Components")]
    public Image playerHPBar;
    public Image playerHPBarBack;
    public Image playerBTBar;
    public Image playerBTBarBack;
    public bool isPaused;
    public int enemyCount = 1;
    public GameObject playerBPUI;
    public GameObject playerFlashDamage;
    public Image redScreenImage;
    public bool isTransitioning = false;
    Color startColor;
    [SerializeField] public GameObject dashMes;
    [SerializeField] public GameObject grindMes;
    [SerializeField] public GameObject BTMessage;

    [Header("Object Access")]
    public GameObject player;
    public wallRun playerScript;
    public enemyAI AIScript;
    public GrindScript grindScript;
    public bool doorIsDestroyable = false;
    [SerializeField] GameObject messagePanel;
    public GameObject keyRejection;
    public GameObject keyAcceptance;
    public KeyTurnIn keys;
    [SerializeField] AudioSource ambientMusic;
    [SerializeField] AudioSource battleMusic;
    [SerializeField] TransitionMusic musicChanger;
    public bool inBattle = false;
    //public bool inBoss;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;
    public float sensitivity;

    [SerializeField] bool boss2Scene = false;
    public BossManager bossManager;
    public AudioController audioScript;

    public GameObject playerPushBack;
    public int Knifecount;
    public JointDamage jointCS;
    [SerializeField] TMP_Text KnifeCountText;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (boss2Scene)
        {
            bossManager = GameObject.FindWithTag("BossManager").GetComponent<BossManager>();
        }
        InitializeSettings();
        InitializePlayer();
    }

    private void Start()
    {
        Debug.Log("GameManager Start called");

        startColor = redScreenImage.color;
        startColor.a = 0f;
        redScreenImage.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattle)
        {
            Debug.Log("Battle");
            musicChanger.musictochange = battleMusic;
            musicChanger.ChangeSong();
        }
        else
        {
            Debug.Log("Ambient");
            musicChanger.musictochange = ambientMusic;
            musicChanger.ChangeSong();
        }

        if (Input.GetButtonDown("Cancel") && SceneManager.GetActiveScene().name != "Main Menu")
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
                exitMenu.SetActive(false);
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
                menuSettings.SetActive(false);
            }
        }

    }

    public void InitializeSettings()
    {
        LoadSettings();
        if (sensSlider != null)
        {
            Debug.Log("Setting up sensitivity slider listener");
            sensSlider.onValueChanged.AddListener(SetSensitivity);
            sensSlider.value = sensitivity;
        }
    }

    public void LoadSettings()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        isInverted = PlayerPrefs.GetInt("InvertMouse", 0) == 1;

        Debug.Log("Loaded Settings - Sensitivity: " + sensitivity + ", Inverted: " + isInverted);

        if (invertON != null && invertOFF != null)
        {
            invertON.SetActive(isInverted);
            invertOFF.SetActive(!isInverted);
        }

        if (sensSlider != null)
        {
            sensSlider.value = sensitivity;
        }
    }

    public void SaveSettings()
    {

        Debug.Log("Saved Settings - Sensitivity: " + sensitivity + ", Inverted: " + isInverted);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.SetInt("InvertMouse", isInverted ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Settings Saved");
    }


    public void ToggleInvertMouse()
    {
        isInverted = !isInverted;
        invertON.SetActive(isInverted);
        invertOFF.SetActive(!isInverted);
        SaveSettings();
    }

    public void SetSensitivity(float value)
    {
        Debug.Log("Set Sensitivity - New Value: " + value);
        sensitivity = value;
        Debug.Log("Sensitivity after update: " + sensitivity);
        SaveSettings();
    }

    private void InitializePlayer()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<wallRun>();
            grindScript = player.GetComponent<GrindScript>();
            audioScript = player.GetComponent<AudioController>();
        }
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
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



    public void TriggerRedToBlackScreen()
    {
        if (!isTransitioning)
        {
            StartCoroutine(RedToBlackTransition());
        }
    }

    IEnumerator RedToBlackTransition()
    {
        isTransitioning = true;

        playerHPBarBack.enabled = false;

        float duration = 1f;
        float timer = 0f;

        //Fade to red over 1 second
        Color startColor = new Color(1f, 0f, 0f, 0f); // Fully transparent red
        Color midColor = new Color(1f, 0f, 0f, 1f); // Fully opaque red

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            redScreenImage.color = Color.Lerp(startColor, midColor, t);
            yield return null;
        }

        // Ensure the color is exactly midColor after the loop
        redScreenImage.color = midColor;

        // Reset the timer for the next transition
        timer = 0f;

        //Fade to black over 1 second
        Color endColor = new Color(0f, 0f, 0f, 1f); // Fully opaque black

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            redScreenImage.color = Color.Lerp(midColor, endColor, t);
            yield return null;
        }

        // Ensure the color is exactly endColor after the loop
        redScreenImage.color = endColor;

        isTransitioning = false;

        youLose();
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

    public void SwitchToSettings()
    {
        UI.SetActive(false);
        menuSettings.SetActive(true);
    }

    public void SwitchToUI()
    {
        UI.SetActive(true);
        menuSettings.SetActive(false);
    }

    void DisablePanels()
    {
        PanelControls.SetActive(false);
        PanelVideo.SetActive(false);
        PanelGame.SetActive(false);
        PanelKeyBindings.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
        lineVideo.SetActive(false);
        lineKeyBindings.SetActive(false);

        PanelMovement.SetActive(false);
        //lineMovement.SetActive(false);
        PanelCombat.SetActive(false);
        //lineCombat.SetActive(false);
        PanelGeneral.SetActive(false);
        lineGeneral.SetActive(false);
    }

    public void GamePanel()
    {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }

    public void VideoPanel()
    {
        DisablePanels();
        PanelVideo.SetActive(true);
        lineVideo.SetActive(true);
    }

    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }

    public void SkillsPanel()
    {
        DisablePanels();
        MovementPanel();
        PanelKeyBindings.SetActive(true);
        lineKeyBindings.SetActive(true);
    }

    public void MovementPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelMovement.SetActive(true);
        //lineMovement.SetActive(true);
    }

    public void CombatPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelCombat.SetActive(true);
        //lineCombat.SetActive(true);
    }

    public void GeneralPanel()
    {
        DisablePanels();
        PanelKeyBindings.SetActive(true);
        PanelGeneral.SetActive(true);
        lineGeneral.SetActive(true);
    }

    public void updateKnifeCount(int amount)
    {
        Knifecount = amount;
        KnifeCountText.text = Knifecount.ToString("F0");
    }

}