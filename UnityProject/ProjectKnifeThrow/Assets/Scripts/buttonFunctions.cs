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

    public void settings()
    {
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

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void invertMouse()
    {
        bool isCurrentlyActive = GameManager.instance.invertON.activeSelf;
        GameManager.instance.invertON.SetActive(!isCurrentlyActive);
        GameManager.instance.invertOFF.SetActive(isCurrentlyActive);
    }
}
