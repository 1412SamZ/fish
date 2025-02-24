using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryCategoryHandlerWithDoTween : MonoBehaviour
{
    public Button inventoryButton;
    public GameObject inventoryPanel;
    public Button fishCategoryButton;
    public Button landscapeCategoryButton;
    public Button exitPanelButton; // 新增退出按钮

    // 平级的 fish 和 landscape 面板
    public GameObject fishPanel;
    public GameObject landscapePanel;

    // 可调节的子面板位置
    public Vector2 subPanelPosition;

    private GameObject currentPanel;

    void Start()
    {
        inventoryPanel.SetActive(false);
        fishPanel.SetActive(false);
        landscapePanel.SetActive(false);

        // 设置子面板的位置
        fishPanel.GetComponent<RectTransform>().anchoredPosition = subPanelPosition;
        landscapePanel.GetComponent<RectTransform>().anchoredPosition = subPanelPosition;
    
        Debug.Log($"fishPanel 锚点位置: {fishPanel.GetComponent<RectTransform>().anchoredPosition}");
        Debug.Log($"landscapePanel 锚点位置: {landscapePanel.GetComponent<RectTransform>().anchoredPosition}");
    
        inventoryButton.onClick.AddListener(ShowInventoryPanel);
        fishCategoryButton.onClick.AddListener(() => SwitchPanel(fishPanel));
        landscapeCategoryButton.onClick.AddListener(() => SwitchPanel(landscapePanel));
        exitPanelButton.onClick.AddListener(HideInventoryPanel);
    
        // 检查 CanvasGroup 组件
        if (inventoryPanel.GetComponent<CanvasGroup>() == null)
        {
            Debug.LogError("inventoryPanel 缺少 CanvasGroup 组件，请添加！");
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
                fishPanel.SetActive(false);
                landscapePanel.SetActive(false);
            });
        }
        else
        {
            Debug.Log("inventoryPanel 未激活，无需隐藏");
        }
    }
}
