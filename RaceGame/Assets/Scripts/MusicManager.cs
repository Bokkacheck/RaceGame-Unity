using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] pesme;
    [SerializeField]
    private AudioSource izvor;
    [SerializeField]
    private Slider glasnoca;

    void Start()
    {
        izvor.loop = false;
        glasnoca.onValueChanged.AddListener(delegate { setVolume(); });
    }

    // Update is called once per frame
    void Update()
    {
        if(!izvor.isPlaying)
        {
            izvor.clip=randomPesma();
            izvor.Play();
        }
    }

    private AudioClip randomPesma()
    {
        return pesme[UnityEngine.Random.Range(0, pesme.Length)];
    }

    

    private void setVolume()
    {
        izvor.volume = glasnoca.value;
    }

}
