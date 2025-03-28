using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] bgmTracks; // List of available tracks
    public string[] trackTitles;  // Corresponding track titles
    public string[] trackCredits; // Corresponding credits for each track

    public MusicToggleSync musicToggleSync; // Reference to sync toggle across screens
    public Text trackInfoText; 

    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicToggleSync != null)
        {
            foreach (var toggle in musicToggleSync.toggles)
            {
                if (toggle != null)
                {
                    toggle.onValueChanged.AddListener(ToggleMusic);
                }
            }
        }

        audioSource.loop = true;

        // Ensure we have at least one track before playing
        if (bgmTracks.Length > 0)
        {
            currentTrackIndex = Random.Range(0, bgmTracks.Length);
            PlayCurrentTrack();
        }
    }

    // Plays the currently selected track
    void PlayCurrentTrack()
    {
        if (currentTrackIndex >= 0 && currentTrackIndex < bgmTracks.Length)
        {
            audioSource.clip = bgmTracks[currentTrackIndex];
            audioSource.Play();
            UpdateTrackUI(); // Update UI with new track info
        }
    }

    // Play a new random track, ensuring it's different from the last one
    public void PlayRandomTrack()
    {
        if (bgmTracks.Length == 0)
        {
            Debug.LogWarning("No BGM found!");
            return;
        }

        int newTrackIndex;
        do
        {
            newTrackIndex = Random.Range(0, bgmTracks.Length);
        } while (newTrackIndex == currentTrackIndex); // Prevents repeating the same track

        currentTrackIndex = newTrackIndex;
        PlayCurrentTrack();
    }

    // Toggle music on/off from UI toggle
    void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    // Check if music is playing
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    // Get formatted track info
    public string GetCurrentTrackInfo()
    {
        if (currentTrackIndex >= 0 && currentTrackIndex < bgmTracks.Length)
        {
            return $"Playing: {trackTitles[currentTrackIndex]}\n Composed by {trackCredits[currentTrackIndex]}";
        }
        return "No Track Playing";
    }

    // Update UI text with the current track info
    void UpdateTrackUI()
    {
        if (trackInfoText != null)
        {
            trackInfoText.text = GetCurrentTrackInfo();
        }
    }
}
