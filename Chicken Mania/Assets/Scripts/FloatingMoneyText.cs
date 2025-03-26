using UnityEngine;
using TMPro;

public class FloatingMoneyText : MonoBehaviour
{
    public string value;
    public Color colour;
    private static GameObject _prefab;
    public static GameObject prefab { get
        {
            if (_prefab == null)
            {
                _prefab = Resources.Load<GameObject>("FloatingMoneyText");
            }
            return _prefab;
        } }
    private RectTransform RTransform;
    private TextMeshPro TM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RTransform = GetComponent<RectTransform>();
        TM = GetComponent<TextMeshPro>();

        TM.text = value;
        TM.color = colour;


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

    // Pass in message and a transform's location to spawn the text at. Col is colour of the text. Prefix is optional, use it if gained or lost money, usually + or -.
    public static void SpawnText(string message, Vector3 location, Color col, string prefix = "")
    {
        location = location + new Vector3(0.0f, 0.0f, 0.2f);
        Quaternion rot = Quaternion.AngleAxis(55, Vector3.right);

        GameObject moneyText = Instantiate(prefab, location, rot);
        FloatingMoneyText moneyTextScript = moneyText.GetComponent<FloatingMoneyText>();

        if (prefix == "")
        {
            moneyTextScript.value = prefix + message;

        }
        else
        {
            moneyTextScript.value = prefix + message + "$";
        }

        moneyTextScript.colour = col;
    }
}
