using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    public void resume()
    {
        GameManager.instance.stateUnPause();
    }

    public void inGameSettings()
    {
        GameManager.instance.SwitchToSettings();
    }

    public void settings()
    {
        GameManager.instance.playMenu.SetActive(false);
        GameManager.instance.menuLevelSelect.SetActive(false);
        GameManager.instance.exitMenu2.SetActive(false);
        GameManager.instance.SwitchToSettings();
    }

    public void Return()
    {
        GameManager.instance.SwitchToUI();
    }

    public void respawn()
    {

        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.stateUnPause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnPause();
    }

    public void onPlayClick()
    {
        GameManager.instance.playMenu.SetActive(true);
        GameManager.instance.exitMenu2.SetActive(false);
    }

    public void onLevelSelect()
    {
        GameManager.instance.menuLevelSelect.SetActive(true);
    }

    public void onExitLevelClick()
    {
        GameManager.instance.exitMenu.SetActive(true);
        GameManager.instance.exitMenu1.SetActive(true);
    }

    public void onExitClick()
    {
        GameManager.instance.playMenu.SetActive(false);
        GameManager.instance.menuLevelSelect.SetActive(false);
        GameManager.instance.exitMenu2.SetActive(true);
    }

    public void onExitnotSureClick()
    {
        if (GameManager.instance.exitMenu != null)
        {
            GameManager.instance.exitMenu.SetActive(false);
        }
        if (GameManager.instance.exitMenu1 != null)
        {
            GameManager.instance.exitMenu1.SetActive(false);
        }
        if (GameManager.instance.exitMenu2 != null)
        {
            GameManager.instance.exitMenu2.SetActive(false);
        }
    }
    public void toCredits()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Credits"));
        SceneManager.LoadScene("Credits");
        GameManager.instance.InitializeSettings();
    }

    public void toMain()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Main Menu"));
        SceneManager.LoadScene("Main Menu");
        GameManager.instance.InitializeSettings();
    }

    public void tutorialLevel()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Tutorial"));
        GameManager.instance.InitializeSettings();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void level1()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Level1"));
        GameManager.instance.InitializeSettings();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void level2()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Level2"));
        GameManager.instance.InitializeSettings();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void level3()
    {
        StartCoroutine(GameManager.instance.StartLoadingCoroutine("Level3"));
        GameManager.instance.InitializeSettings();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void OnInvertMouseToggle()
    {
        GameManager.instance.ToggleInvertMouse();
    }

    public void OnSensitivityChanged(float value)
    {
        GameManager.instance.SetSensitivity(value);
        Debug.Log("Sensitivity Changed - New Value: " + value);
    }
}
