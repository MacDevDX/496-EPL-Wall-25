using UnityEngine;

public class HorizontalTextScroll : MonoBehaviour
{
    public float scrollSpeed = 50f; // Speed of text scrolling
    private RectTransform textRectTransform;

    private void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Move the text horizontally
        textRectTransform.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;

        // Reset position when out of bounds (inside the TextContainer)
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        if (textRectTransform.localPosition.x < -textRectTransform.rect.width)
        {
            textRectTransform.localPosition = new Vector3(parentRect.rect.width, textRectTransform.localPosition.y, textRectTransform.localPosition.z);
        }
    }
}
