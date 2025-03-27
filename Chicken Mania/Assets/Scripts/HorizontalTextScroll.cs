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
        textRectTransform.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;

        //Reset if outside the MusicBoxText container
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        if (textRectTransform.localPosition.x < -textRectTransform.rect.width)
        {
            textRectTransform.localPosition = new Vector3(parentRect.rect.width, textRectTransform.localPosition.y, textRectTransform.localPosition.z);
        }
    }
}
