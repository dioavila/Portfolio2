using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    [SerializeField] GameObject ActiveMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject WinMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TMP_Text text;
    public GameObject flashdamage;
    public GameObject player;
    public Image HPbar;
    public PlayerProjectile playerscript;
    public bool ispaused;
    int enemycount;


    // Start is called before the first frame update
    void Awake()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (ActiveMenu == null)
            {
                Pause();
                ActiveMenu = PauseMenu;
                ActiveMenu.SetActive(ispaused);
            }
            else
            {
                unPause();
            }

        }
    }
    public void Pause()
    {
        ispaused = !ispaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        ispaused = !ispaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ActiveMenu.SetActive(ispaused);
        ActiveMenu = null;
    }

}
