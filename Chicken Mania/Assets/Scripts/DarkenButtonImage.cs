using UnityEngine;
using UnityEngine.UI;

public class DarkenButtonImage : MonoBehaviour
{
    private Button button;
    private Image buttonImage;

    [Range(0f, 1f)]
    public float darkenAmount = 0.5f; // % to darken the image

    private Color originalColor;

    // Reference to the shop manager
    public ShopManager shopManager;

    // The type of button
    public enum ButtonType { Sell, Buy, Upgrade, ManiaSettings, TycoonSettings, Home, HatchSettings, TimedHome, ProtectSettings }
    public ButtonType buttonType;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
        {
            originalColor = buttonImage.color; // Store the original color
        }
    }

    void Update()
    {
        // Check the state on the button type
        bool isActive = false;

        if (shopManager != null)
        {
            switch (buttonType)
            {
                case ButtonType.Sell:
                    isActive = shopManager.IsSellZoneActive();
                    break;
                case ButtonType.Buy:
                    isActive = shopManager.ShopWindow != null && shopManager.ShopWindow.activeSelf;
                    break;
                case ButtonType.Upgrade:
                    isActive = shopManager.UpgradeWindow != null && shopManager.UpgradeWindow.activeSelf;
                    break;
                case ButtonType.ManiaSettings:
                    isActive = shopManager.SettingsButtonMenuMania != null && shopManager.SettingsButtonMenuMania.activeSelf;
                    break;
                case ButtonType.TycoonSettings:
                    isActive = shopManager.SettingsButtonMenuTycoon != null && shopManager.SettingsButtonMenuMania.activeSelf;
                    break;
                case ButtonType.Home:
                    isActive = shopManager.HomeButtonMenu != null && shopManager.HomeButtonMenu.activeSelf;
                    break;
                case ButtonType.HatchSettings:
                    isActive = shopManager.HatchSettingsButtonMenu != null && shopManager.HatchSettingsButtonMenu.activeSelf;
                    break;
                case ButtonType.TimedHome:
                    isActive = shopManager.TimedHomeButtonMenu != null && shopManager.TimedHomeButtonMenu.activeSelf;
                    break;
                case ButtonType.ProtectSettings:
                    isActive = shopManager.ProtectSettingsButtonMenu != null && shopManager.ProtectSettingsButtonMenu.activeSelf;
                    break;
            }
        }

        // change the button image's color based on the active state
        if (buttonImage != null)
        {
            buttonImage.color = isActive
                ? new Color(originalColor.r * darkenAmount, originalColor.g * darkenAmount, originalColor.b * darkenAmount, originalColor.a)
                : originalColor;
        }
    }
}
