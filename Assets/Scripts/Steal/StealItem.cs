using UnityEngine;

public enum ItemType
{
    Wallet, Bag, OfficeBag, Ruby, Emerald ,Diamond, Carkey, Cellphone, GoldCoin, Neckless, Watch
}

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/StealItem", order = 1)]
public class StealItem: ScriptableObject
{
    public ItemType ItemType;
    public Sprite ItemSprite;
    public int ItemValue;
    public int ItemWeight;
}