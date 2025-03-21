using UnityEngine;
using TMPro;

public class MusicUIUpdater : MonoBehaviour
{
    public MusicHandler musicHandler; // Reference to the MusicHandler
    public TextMeshProUGUI[] trackInfoTexts; // All UI text objects that will display the music info
    void Start()
    {
        UpdateUI(); // Update UI when the game starts
    }

    void Update()
    {
        UpdateUI(); // Update UI to reflect current track
    }

    void UpdateUI()
    {
        if (musicHandler != null && trackInfoTexts.Length > 0)
        {
            string trackInfo = musicHandler.GetCurrentTrackInfo();
            foreach (var textUI in trackInfoTexts)
            {
                if (textUI != null)
                {
                    textUI.text = trackInfo;
                }
            }
        }
    }
}
