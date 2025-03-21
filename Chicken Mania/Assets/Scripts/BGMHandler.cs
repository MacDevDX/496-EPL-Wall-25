using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class RandomBGM : MonoBehaviour
{
    public AudioClip[] bgmTracks;
    public SliderSync sliderSync;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (sliderSync != null)
        {
            //Add all slider objects from SyncSlider
            foreach (var slider in sliderSync.sliders)
            {
                if (slider != null)
                {
                    slider.onValueChanged.AddListener(ChangeVolume);

                    LongPressGesture longPressGesture = slider.gameObject.GetComponent<LongPressGesture>();
                    longPressGesture.StateChanged += OnLongPress;
                }
            }
        }

        audioSource.loop = true;
        PlayRandomTrack();

    }

    void PlayRandomTrack()
    {
        if (bgmTracks.Length == 0)
        {
            Debug.LogWarning("No BGM found!");
            return;
        }

        int randomIndex = Random.Range(0, bgmTracks.Length);
        audioSource.clip = bgmTracks[randomIndex];
        audioSource.Play();
    }

    void ChangeVolume(float value)
    {
        audioSource.volume = value; //Set volume based on slider value
    }

    private void OnLongPress(object sender, GestureStateChangeEventArgs e)
    {
        if (e.State == Gesture.GestureState.Recognized)
        {
            PlayRandomTrack(); //Change tracks if long press on slider
        }
    }
}


