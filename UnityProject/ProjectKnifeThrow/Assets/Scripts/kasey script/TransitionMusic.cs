using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//change music in scene

public class TransitionMusic : MonoBehaviour
{
    [SerializeField] public AudioSource musictochange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeSong();
        }
    }

    public void ChangeSong()
    {
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager == null) 
        {
            return;
        }
        if (musicManager.CurrSong == null)
        {
            musicManager.CurrSong = musictochange;
            musicManager.CurrSong.mute = false;
            return;
        }
        musicManager.CurrSong.mute = true;
        musicManager.CurrSong = musictochange;
        musicManager.CurrSong.mute = false;
    }
}
