using UnityEngine;
using UnityEngine.UI;

public class MusicToggleSync : MonoBehaviour
{

    public Toggle[] toggles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var toggle in toggles)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((isOn) => SyncToggles(toggle, isOn));
            }
        }
        
    }

    void SyncToggles(Toggle changedToggle, bool isOn)
    {
        // Update all toggles except the one that triggered the change
        foreach (var toggle in toggles)
        {
            if (toggle != null && toggle != changedToggle)
            {
                toggle.isOn = isOn;
            }
        }
    }
}
