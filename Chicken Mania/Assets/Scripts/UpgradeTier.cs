[System.Serializable]
public class UpgradeTier
{
    public int ShopID;
    public int Price;
    public int Level;

    public UpgradeTier(int shopID, int price, int level)
    {
        ShopID = shopID;
        Price = price;
        Level = level;
    }
}