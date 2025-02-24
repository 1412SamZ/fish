using System.Collections.Generic;
using UnityEngine;

// 物品类，代表鱼苗、设备等
[System.Serializable]
public class Item
{
    public string itemName;
    public int itemId;
}

// 卡牌类，每张卡牌对应一个物品
[System.Serializable]
public class FishCard
{
    public string cardName;
    public Item associatedItem;
}

// 福袋类，包含若干张卡牌
[System.Serializable]
public class FishingBag
{
    public string bagName;
    public List<FishCard> cards;
}
