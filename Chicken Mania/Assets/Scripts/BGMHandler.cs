using UnityEngine;
using UnityEngine.UI;

public class RandomBGM : MonoBehaviour
{
    public AudioClip[] bgmTracks;
    public Slider volumeSlider;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

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
        audioSource.volume = value; // Set volume based on slider value
    }
}
