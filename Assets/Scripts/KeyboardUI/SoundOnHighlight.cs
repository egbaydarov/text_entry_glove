using LeapMotionGesture;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnHighlight : MonoBehaviour, IPointerEnterHandler
{
#pragma warning disable 649
    [SerializeField]
    private new AudioSource audio;
    [SerializeField]
    private AudioClip soundToPlay;
#pragma warning restore 649

    private void Start()
    {
        audio = FindObjectOfType<AudioSource>();
        if (audio == null)
        {
            Debug.LogError("SoundOnHighlight: The 'Audio' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (soundToPlay == null)
        {
            Debug.LogError("SoundOnHighlight: The 'SoundToPlay' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enabled || AirStrokeMapper.pinchIsOn)
            return;

        audio.PlayOneShot(soundToPlay);
    }
}
