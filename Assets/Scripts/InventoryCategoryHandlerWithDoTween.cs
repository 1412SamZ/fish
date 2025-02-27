using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

// 类别数据类
public class CategoryData
{
    public Button categoryButton;
    public GameObject categoryPanel;
    public Transform listContent;
    public List<InventoryItemData> itemDataList;
    public ScrollRect scrollRect; // 新增 ScrollRect 字段
}

public class InventoryCategoryHandlerWithDoTween : MonoBehaviour
{
    public Button inventoryButton;
    public GameObject inventoryPanel;
    public Button exitPanelButton; // 新增退出按钮
    public GameObject inventoryItemPrefab;
    public Vector2 subPanelPosition;

    // 序列化字段，在 Inspector 面板中手动指定鱼类相关对象
    [SerializeField] private Button fishButton;
    [SerializeField] private GameObject fishPanel;
    [SerializeField] private Transform fishListContent;
    [SerializeField] private ScrollRect fishScrollRect; // 新增鱼类 ScrollRect 字段

    // 序列化字段，在 Inspector 面板中手动指定景观相关对象
    [SerializeField] private Button landscapeButton;
    [SerializeField] private GameObject landscapePanel;
    [SerializeField] private Transform landscapeListContent;
    [SerializeField] private ScrollRect landscapeScrollRect; // 新增景观 ScrollRect 字段

    private GameObject currentPanel;
    private List<CategoryData> categoryDataList = new List<CategoryData>();

    void Start()
    {
        inventoryPanel.SetActive(false);
    
        // 检查 CanvasGroup 组件
        if (inventoryPanel.GetComponent<CanvasGroup>() == null)
        {
            Debug.LogError("inventoryPanel 缺少 CanvasGroup 组件，请添加！");
        }
    
        // 初始化类别数据
        InitializeCategoryData();
    
        // 设置子面板的位置
        foreach (var categoryData in categoryDataList)
        {
            categoryData.categoryPanel.SetActive(false);
            categoryData.categoryPanel.GetComponent<RectTransform>().anchoredPosition = subPanelPosition;
            Debug.Log($"{categoryData.categoryPanel.name} 锚点位置: {categoryData.categoryPanel.GetComponent<RectTransform>().anchoredPosition}");
        }
    
        // 调整 ScrollRect 的大小
        float itemWidth = inventoryItemPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float itemHeight = inventoryItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float spacing = 10f; // 项之间的间距
        float twoColumnWidth = 2 * itemWidth + spacing;
        float fourRowHeight = 4 * (itemHeight + spacing);
    
        fishScrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(twoColumnWidth, fourRowHeight);
        landscapeScrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(twoColumnWidth, fourRowHeight);
    
        // 禁用水平滚动
        fishScrollRect.horizontal = false;
        landscapeScrollRect.horizontal = false;
    
        inventoryButton.onClick.AddListener(ShowInventoryPanel);
        exitPanelButton.onClick.AddListener(HideInventoryPanel);
    
        // 为每个类别按钮添加点击事件
        foreach (var categoryData in categoryDataList)
        {
            categoryData.categoryButton.onClick.AddListener(() => SwitchPanel(categoryData.categoryPanel));
            AddItemsToList(categoryData.listContent, categoryData.itemDataList, categoryData.scrollRect); // 修改方法调用
        }
    }

    void InitializeCategoryData()
    {
        // 鱼类类别数据
        if (fishButton == null || fishPanel == null || fishListContent == null || fishScrollRect == null)
        {
            Debug.LogError("鱼类类别数据相关对象未在 Inspector 面板中指定，请检查！");
            return;
        }

        CategoryData fishCategory = new CategoryData
        {
            categoryButton = fishButton,
            categoryPanel = fishPanel,
            listContent = fishListContent,
            itemDataList = new List<InventoryItemData>(),
            scrollRect = fishScrollRect // 赋值 ScrollRect
        };

        for (int i = 1; i <= 12; i++)
        {
            fishCategory.itemDataList.Add(new InventoryItemData
            {
                Icon = null,
                Name = $"Fish {i}",
                Description = $"This is fish {i}"
            });
        }

        categoryDataList.Add(fishCategory);

        // 景观类别数据
        if (landscapeButton == null || landscapePanel == null || landscapeListContent == null || landscapeScrollRect == null)
        {
            Debug.LogError("景观类别数据相关对象未在 Inspector 面板中指定，请检查！");
            return;
        }

        CategoryData landscapeCategory = new CategoryData
        {
            categoryButton = landscapeButton,
            categoryPanel = landscapePanel,
            listContent = landscapeListContent,
            itemDataList = new List<InventoryItemData>(),
            scrollRect = landscapeScrollRect // 赋值 ScrollRect
        };

        for (int i = 1; i <= 12; i++)
        {
            landscapeCategory.itemDataList.Add(new InventoryItemData
            {
                Icon = null,
                Name = $"Landscape {i}",
                Description = $"This is landscape {i}"
            });
        }

        categoryDataList.Add(landscapeCategory);

        // 可以在这里添加更多类别数据
    }

    void AddItemsToList(Transform content, List<InventoryItemData> dataList, ScrollRect scrollRect)
    {
        if (content == null)
        {
            Debug.LogError("Content transform is null!");
            return;
        }
    
        if (scrollRect == null)
        {
            Debug.LogError($"ScrollRect for {content.name} is null!");
            return;
        }
    
        // 确保 Viewport 有 Mask 组件
        RectTransform viewportRect = scrollRect.viewport;
        if (viewportRect != null)
        {
            Mask viewportMask = viewportRect.GetComponent<Mask>();
            if (viewportMask == null)
            {
                viewportMask = viewportRect.gameObject.AddComponent<Mask>();
                viewportMask.showMaskGraphic = false;
            }
        }
    
        float itemWidth = inventoryItemPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float itemHeight = inventoryItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float spacing = 10f; // 项之间的间距
    
        // 获取或添加 Grid Layout Group 组件
        GridLayoutGroup gridLayout = content.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = content.gameObject.AddComponent<GridLayoutGroup>();
        }
    
        // 设置 Grid Layout Group 参数
        gridLayout.cellSize = new Vector2(itemWidth, itemHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 2;
    
        for (int i = 0; i < dataList.Count; i++)
        {
            GameObject item = Instantiate(inventoryItemPrefab, content);
            InventoryItemData data = dataList[i];
    
            // 设置图标
            Image iconImage = item.transform.Find("IconImage")?.GetComponent<Image>();
            if (iconImage == null)
            {
                Debug.LogError($"在 {item.name} 中找不到 IconImage 组件！");
                continue;
            }
            iconImage.sprite = data.Icon;
    
            // 设置名字
            TextMeshProUGUI nameText = item.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            if (nameText == null)
            {
                Debug.LogError($"在 {item.name} 中找不到 NameText 组件！");
                continue;
            }
            nameText.text = data.Name;
    
            // 设置注释
            TextMeshProUGUI descriptionText = item.transform.Find("DescriptionText")?.GetComponent<TextMeshProUGUI>();
            if (descriptionText == null)
            {
                Debug.LogError($"在 {item.name} 中找不到 DescriptionText 组件！");
                continue;
            }
            descriptionText.text = data.Description;
    
            // 计算两列的宽度
            float twoColumnWidth = 2 * itemWidth + spacing;
            // 计算四行的高度
            float fourRowHeight = 4 * (itemHeight + spacing);
    
            // 调整 Content 的大小为 2 列 4 行
            RectTransform contentRect = content.GetComponent<RectTransform>();
            contentRect.sizeDelta = new Vector2(twoColumnWidth, fourRowHeight);
    
            // 设置 Content 的锚点和轴心点
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(0, 1);
            contentRect.pivot = new Vector2(0, 1);
            contentRect.anchoredPosition = Vector2.zero;
    
            if (viewportRect != null)
            {
                // 禁用可能影响布局的组件
                ContentSizeFitter contentSizeFitter = viewportRect.GetComponent<ContentSizeFitter>();
                if (contentSizeFitter != null)
                {
                    contentSizeFitter.enabled = false;
                }
    
                // 设置视口的宽度为两列宽，高度为四行高
                viewportRect.sizeDelta = new Vector2(twoColumnWidth, fourRowHeight);
    
                // 设置 Viewport 的锚点和轴心点
                viewportRect.anchorMin = new Vector2(0, 1);
                viewportRect.anchorMax = new Vector2(0, 1);
                viewportRect.pivot = new Vector2(0, 1);
                viewportRect.anchoredPosition = Vector2.zero;
    
                // 滚动到顶部
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }
    }
    void ShowInventoryPanel()
    {
        inventoryPanel.SetActive(true);
        inventoryPanel.GetComponent<CanvasGroup>().alpha = 0;
        inventoryPanel.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.OutQuad);
    }

    void SwitchPanel(GameObject targetPanel)
    {
        if (currentPanel != null)
        {
            currentPanel.transform.DOLocalMoveX(-Screen.width, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                currentPanel.SetActive(false);
                // 等旧面板隐藏动画完成后，再显示新面板
                ShowTargetPanel(targetPanel);
            });
        }
        else
        {
            // 如果没有当前面板，直接显示目标面板
            ShowTargetPanel(targetPanel);
        }
    }

    void ShowTargetPanel(GameObject targetPanel)
    {
        targetPanel.SetActive(true);
        // 设置初始位置为 subPanelPosition
        targetPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(subPanelPosition.x + Screen.width, subPanelPosition.y);
        // 动画移动到目标位置
        targetPanel.GetComponent<RectTransform>().DOAnchorPosX(subPanelPosition.x, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 动画完成后再次确认位置
            targetPanel.GetComponent<RectTransform>().anchoredPosition = subPanelPosition;
        });

        currentPanel = targetPanel;
    }

    void HideInventoryPanel()
    {
        Debug.Log("点击了退出按钮，尝试隐藏 inventoryPanel");
        if (inventoryPanel.activeSelf)
        {
            Debug.Log("inventoryPanel 处于激活状态，开始淡出动画");
            inventoryPanel.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Debug.Log("淡出动画完成，隐藏 inventoryPanel");
                inventoryPanel.SetActive(false);
                currentPanel = null;
                foreach (var categoryData in categoryDataList)
                {
                    categoryData.categoryPanel.SetActive(false);
                }
            });
        }
        else
        {
            Debug.Log("inventoryPanel 未激活，无需隐藏");
        }
    }
}
