using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource CurrentSong;

    public AudioSource CurrSong
    {
        get => CurrentSong;
        set => CurrentSong = value;
    }
}
