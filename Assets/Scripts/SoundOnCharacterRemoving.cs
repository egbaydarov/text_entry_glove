using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCharacterRemoving : MonoBehaviour
{
    string prevValue = "";

    [SerializeField]
    private new AudioSource audio;

    [SerializeField]
    private AudioClip soundToPlay;

    public static bool IsWrongClickToBackspace = false;

    public static int LastCharCountRemoved { get; set; }

    static int CharCountRemoved;
    public static int GetCharCountRemoved()
    {
        int res = CharCountRemoved - LastCharCountRemoved;
        LastCharCountRemoved = CharCountRemoved;

        return res;
    }

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

        if (!EntryProcessing.IsLastClickPrediction && prevValue.Length > value.Length)
        {
            audio.PlayOneShot(soundToPlay);
            CharCountRemoved += prevValue.Length - value.Length;
            if (value.Length != 0 || value[value.Length - 1] != ' ')
                IsWrongClickToBackspace = true;
        }

        prevValue = value;
    }
}
