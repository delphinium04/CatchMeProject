using UnityEngine;
using UnityEngine.Serialization;

// 2: Cellphone~ - 3: Neckless~
public enum ItemType
{
    Wallet, Handbag, Officebag, Ruby, Emerald, Diamond, Cellphone, Watch, Carkey, Neckless, Gold
}

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/StealItem", order = 1)]
public class StealItem: ScriptableObject
{
    public ItemType ItemType;
    public Sprite ItemSprite;
    public int ItemValue => IsRandomValue ? Random.Range(RandomMin, RandomMax+1) : _itemValue;
    [SerializeField] private int _itemValue;
    public int ItemWeight;

    [Header("Random")]
    public bool IsRandomValue;
    public int RandomMin;
    public int RandomMax;
}