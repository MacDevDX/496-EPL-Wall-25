using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasLights : MonoBehaviour
{
    public List<GameObject> redLights;
    public List<GameObject> blueLights;
    public List<GameObject> greenLights;

    public Color redOn = new Color(255 / 255f, 0, 0); 
    public Color redDim = new Color(150 / 255f, 0, 0);

    public Color blueOn = new Color(0, 0, 255 / 255f);
    public Color blueDim = new Color(0, 0, 150 / 255f);

    public Color greenOn = new Color(0, 255 / 255f, 0);
    public Color greenDim = new Color(0, 150 / 255f, 0);

    private Renderer redRenderer, blueRenderer, greenRenderer;

    private void Start()
    {
        StartCoroutine(FlashLights());
    }

    private IEnumerator FlashLights()
    {
        while (true)
        {
            //Red off, Blue and Green on
            /*
            redRenderer.material.color = redDim;
            blueRenderer.material.color = blueOn;
            greenRenderer.material.color = greenOn;
            */
            SetLightsColor(redLights, redDim);
            SetLightsColor(blueLights, blueOn);
            SetLightsColor(greenLights, greenOn);
            yield return new WaitForSeconds(1f);


            //Blue off, Red and Green on
            /*
            redRenderer.material.color = redOn;
            blueRenderer.material.color = blueDim;
            greenRenderer.material.color = greenOn;
            */
            SetLightsColor(redLights, redOn);
            SetLightsColor(blueLights, blueDim);
            SetLightsColor(greenLights, greenOn);
            yield return new WaitForSeconds(1f);

            //Green off, Red and Blue on
            /*
            redRenderer.material.color = redOn;
            blueRenderer.material.color = blueOn;
            greenRenderer.material.color = greenDim;
            */
            SetLightsColor(redLights, redOn);
            SetLightsColor(blueLights, blueOn);
            SetLightsColor(greenLights, greenDim);
            yield return new WaitForSeconds(1f);
        }
    }
    private void SetLightsColor(List<GameObject> lights, Color color)
    {
        foreach (GameObject light in lights)
        {
            Renderer renderer = light.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
