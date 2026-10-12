using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("손님 UI")]
    [SerializeField] private GameObject guestTextPanel; // 손님 대화창 부모 오브젝트 (SetActive용)
    [SerializeField] private TMP_Text guestText;
    [SerializeField] private Button submitButton;

    [Header("엔딩 UI (검은 화면 패널)")]
    [SerializeField] private GameObject endingPanel;      // 검은색 배경 패널
    [SerializeField] private TMP_Text endingTitleText;     // 엔딩 제목 ("영업 대성공!" 등)
    [SerializeField] private TMP_Text endingDescText;      // 엔딩 설명 텍스트

    [Header("상단 UI")]
    [SerializeField] private TMP_Text remainingOrdersText;

    [Header("재료 슬롯 UI")]
    [SerializeField] private List<Image> ingredientSlots;

    private readonly ColorID[] allColors = new ColorID[]
    {
        ColorID.Red, ColorID.Orange, ColorID.Yellow, 
        ColorID.Green, ColorID.Blue, ColorID.Navy, ColorID.Purple
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- 대화창 On/Off 및 텍스트 제어 ---
    public void SetGuestTextActive(bool isActive)
    {
        if (guestTextPanel != null) guestTextPanel.SetActive(isActive);
        else if (guestText != null) guestText.gameObject.SetActive(isActive);
    }

    public void SetGuestText(string text)
    {
        if (guestText != null) guestText.text = text;
    }

    public void SetSubmitButtonActive(bool isActive)
    {
        if (submitButton != null) submitButton.gameObject.SetActive(isActive);
    }

    // --- 엔딩 패널 연출 제어 ---
    public void ShowEndingPanel(string title, string description)
    {
        if (endingPanel != null) endingPanel.SetActive(true);
        if (endingTitleText != null) endingTitleText.text = title;
        if (endingDescText != null) endingDescText.text = description;
    }

    public void HideEndingPanel()
    {
        if (endingPanel != null) endingPanel.SetActive(false);
    }

    // --- 상단 남은 주문 수 UI ---
    public void UpdateRemainingOrdersUI(int remaining)
    {
        if (remainingOrdersText != null)
        {
            remainingOrdersText.text = $"남은 주문: {remaining}개";
        }
    }

    // --- 재료 슬롯 UI ---
    public void UpdateIngredientUI(ColorID currentMixture)
    {
        ResetIngredientUI();
        if (currentMixture == ColorID.None) return;

        int slotIndex = 0;
        foreach (ColorID color in allColors)
        {
            if ((currentMixture & color) == color)
            {
                if (slotIndex < ingredientSlots.Count)
                {
                    ingredientSlots[slotIndex].gameObject.SetActive(true);
                    ingredientSlots[slotIndex].color = ColorBlendTable.GetBlendedColor(color);
                    slotIndex++;
                }
            }
        }
    }

    public void ResetIngredientUI()
    {
        foreach (var slot in ingredientSlots)
        {
            if (slot != null)
            {
                slot.color = Color.clear;
                slot.gameObject.SetActive(false);
            }
        }
    }
}