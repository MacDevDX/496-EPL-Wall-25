using UnityEngine;

public class FloatingMoneyText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float randomDirectionSpeed = 0.5f;
    private Vector3 randomDirection;
    void Start()
    {
        randomDirection = new Vector3(
        Random.Range(-1f, 1f),
        0f,
        Random.Range(-1f, 1f)
        ).normalized * randomDirectionSpeed;
    }

    void Update()
    {
        //transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.position += (Vector3.up * floatSpeed + randomDirection) * Time.deltaTime;
    }
}
