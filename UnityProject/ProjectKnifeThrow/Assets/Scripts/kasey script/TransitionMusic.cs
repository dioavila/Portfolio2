using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMusic : MonoBehaviour
{
    [SerializeField] AudioSource musictochange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeSong();
        }
    }

    private void ChangeSong()
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
