using UnityEngine;
using TMPro;

public class FloatingMoneyText : MonoBehaviour
{
    public string value;
    public bool positive;
    private RectTransform RTransform;
    private TextMeshPro TM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RTransform = GetComponent<RectTransform>();
        TM = GetComponent<TextMeshPro>();

        TM.text = value;
        if (positive)
        {
            TM.color = Color.green;
        }
        else
        {
            TM.color = Color.red;
        }

        Invoke("SelfDestruct", 1.0f);
     }

    // Update is called once per frame
    void Update()
    {
        RTransform.Translate(Vector3.up * Time.deltaTime);
    }

    void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
