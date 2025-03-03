using UnityEngine;
using UnityEngine.UI;

public class SliderSync : MonoBehaviour
{
    public Slider[] sliders; // All sliders that need to sync with each other

    void Start()
    {
        // Add a listener to each slider for value changes
        foreach (var slider in sliders)
        {
            if (slider != null)
            {
                slider.onValueChanged.AddListener((value) => SyncSliders(slider, value));
            }
        }
    }

    void SyncSliders(Slider changedSlider, float value)
    {
        // Update all sliders except the one that triggered the change
        foreach (var slider in sliders)
        {
            if (slider != null && slider != changedSlider)
            {
                slider.value = value;
            }
        }
    }
}
