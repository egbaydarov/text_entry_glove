using LeapMotionGesture;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnHighlight : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private new AudioSource audio;
    [SerializeField]
    private AudioClip soundToPlay;

    private void Start()
    {
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
