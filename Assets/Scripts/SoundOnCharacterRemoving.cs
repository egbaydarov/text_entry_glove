﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCharacterRemoving : MonoBehaviour
{
    string prevValue = "";

    [SerializeField]
    private new AudioSource audio;

    [SerializeField]
    private AudioClip soundToPlay;

    void Start()
    {
        if (audio == null)
        {
            Debug.LogError("SoundOnCharacterRemoving: The 'Audio' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (soundToPlay == null)
        {
            Debug.LogError("SoundOnCharacterRemoving: The 'SoundToPlay' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    public void OnValueChanged(string value)
    {
        if (!enabled)
            return;

        if (prevValue.Length > value.Length)
            audio.PlayOneShot(soundToPlay);

        prevValue = value;
    }
}
