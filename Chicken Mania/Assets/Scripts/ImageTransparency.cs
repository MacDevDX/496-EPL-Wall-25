using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageTransparency : MonoBehaviour
{
    public float Speed = 1f;
    public float min = 0.3f;
    public float max = 1f;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float alpha = Mathf.Lerp(min, max, Mathf.PingPong(Time.time * Speed, 1));
        Color currentColor = image.color;
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}