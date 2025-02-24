using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishShop : MonoBehaviour
{
    public GameObject fishShopPanel;    // 购买面板
    public FishData[] fishItems;        // 鱼类数据数组
    public GameObject fishItemTemplate; // 商品项模板
    public Transform shopContent;       // 商品项的父节点（FishShopPanel）

    private int coins = 1000;           // 玩家金币
    GridLayoutGroup gridLayoutGroup;
    void Start()
    {
        // 初始化时隐藏商店界面
        fishShopPanel.SetActive(true);
        // set fish sop location
        // first get rect transform
        RectTransform rectTransform = fishShopPanel.GetComponent<RectTransform>();
        // then set anchor preset to left top
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = new Vector2(0, 400);
        gridLayoutGroup = shopContent.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.cellSize = new Vector2(100, 120);
            gridLayoutGroup.spacing = new Vector2(100f, 10f);
        }
        // 动态生成商品项
        foreach (FishData fish in fishItems)
        {
            GameObject item = Instantiate(fishItemTemplate, shopContent);
            item.SetActive(true);


            // 绑定按钮事件
            Button buyButton = item.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => TryBuyFish(fish));
            Debug.Log("fishItemTemplate");

            // 设置UI元素
            buyButton.transform.Find("Thumbnail").GetComponent<Image>().sprite = fish.thumbnail;
            buyButton.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = fish.name;
            buyButton.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = fish.cost + " Pt.";

            AdjustButtonSize(buyButton,
                buyButton.transform.Find("Thumbnail").GetComponent<Image>(),
                buyButton.transform.Find("NameText").GetComponent<TextMeshProUGUI>(),
                buyButton.transform.Find("PriceText").GetComponent<TextMeshProUGUI>());
        }
    }
    private void AdjustButtonSize(Button button, Image thumbnail, TextMeshProUGUI nameText, TextMeshProUGUI priceText)
    {
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        RectTransform thumbnailRect = thumbnail.GetComponent<RectTransform>();
        RectTransform nameTextRect = nameText.GetComponent<RectTransform>();
        RectTransform priceTextRect = priceText.GetComponent<RectTransform>();

        float paddingX = 50f;
        float paddingY = 50f;
        // buttonRect.sizeDelta = new Vector2(buttonRect.rect.width + paddingX * 2, buttonRect.rect.height + paddingY * 2);
        buttonRect.localScale = new Vector3(1.5f, 1.5f, 1.0f);


    }
    // 打开/关闭商店界面
    public void ToggleShop(bool isOpen)
    {
        fishShopPanel.SetActive(isOpen);
    }

    // 尝试购买鱼类
    public void TryBuyFish(FishData fish)
    {
        Debug.Log("TryBuyFish");
        if (coins >= fish.cost)
        {
            coins -= fish.cost;
            SpawnFish(fish.prefab);
            Debug.Log($"购买了{fish.name}，剩余金币：{coins}");
        }
        else
        {
            Debug.Log("金币不足！");
        }
    }

    // 生成鱼
    private void SpawnFish(GameObject fishPrefab)
    {
        Vector2 spawnPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Instantiate(fishPrefab, spawnPos, Quaternion.identity);
    }
}
