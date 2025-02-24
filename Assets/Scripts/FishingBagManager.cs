using System.Collections.Generic;
using UnityEngine;

public class FishingBagManager : MonoBehaviour
{
    public FishingBag fishingBag;
    public int cardCountToOpen = 3; // 每次打开福袋获得的卡牌数量

    public void PurchaseAndOpenBag()
    {
        Debug.Log("购买并打开福袋: " + fishingBag.bagName);
        List<FishCard> openedCards = OpenBag();
        foreach (FishCard card in openedCards)
        {
            Debug.Log("获得卡牌: " + card.cardName + ", 对应物品: " + card.associatedItem.itemName);
        }
    }

    private List<FishCard> OpenBag()
    {
        List<FishCard> openedCards = new List<FishCard>();
        for (int i = 0; i < cardCountToOpen; i++)
        {
            int randomIndex = Random.Range(0, fishingBag.cards.Count);
            FishCard card = fishingBag.cards[randomIndex];
            openedCards.Add(card);
        }
        return openedCards;
    }
}
