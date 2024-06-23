using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sounds[] musicSounds;
    public AudioSource musicSource;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            PlayMusic("Main");
        }
        else
        {
            PlayMusic("Ambient");
        }
        if (PlayerPrefs.HasKey("Volume"))
        {
            MusicVolume(PlayerPrefs.GetFloat("Volume"));
            GameManager.instance.musicSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        GameManager.instance.InitializeSettings();
    }

    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    //public void SetMasterVolume(float volume)
    //{
    //    AudioListener.volume = Mathf.Clamp01(volume);
    //}
}
