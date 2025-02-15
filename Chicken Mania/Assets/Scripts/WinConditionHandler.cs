using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinConditionHandler : MonoBehaviour
{
    public float winCountdownTime = 10f;
    private bool hasWon = false;
    private float countdownTimer;

    public GameObject winMessageUI;
    public TextMeshProUGUI countdownText;
    public ShopManager shopManager;

    private void Start()
    {
        if (winMessageUI != null)
        {
            winMessageUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!hasWon && shopManager != null && shopManager.Inventory != null && shopManager.Inventory[3, 6] > 0)
        {
            TriggerWinCondition();
        }

        if (hasWon)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = "You have purchased the ultimate chicken!You Won! Continue playing or time out to reset!" + Mathf.Ceil(countdownTimer) + "s";

            if (countdownTimer <= 0)
            {
                winMessageUI.SetActive(false); 
            }
        }
    }

    private void TriggerWinCondition()
    {
        hasWon = true;
        countdownTimer = winCountdownTime;

        if (winMessageUI != null)
        {
            winMessageUI.SetActive(true);
        }
    }
}
