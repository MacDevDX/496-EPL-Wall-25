using UnityEngine;

public class WobbleText : MonoBehaviour
{
    public float wobbleAmount = 10f; // Max horizontal movement for wobble
    public float wobbleSpeed = 2f;   // Speed of wobble

    private RectTransform rectTransform;
    private float startX;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startX = rectTransform.anchoredPosition.x;
    }

    void Update()
    {
        // Apply a wobble effect using a sine wave
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        rectTransform.anchoredPosition = new Vector2(startX + wobble, rectTransform.anchoredPosition.y);
    }
}
