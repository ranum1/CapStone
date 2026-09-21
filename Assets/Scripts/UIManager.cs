using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("손님 UI")]
    [SerializeField] private TMP_Text guestText;
    [SerializeField] private Button submitButton;

    [Header("재료 표시 슬롯 UI (최대 3개)")]
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

    // --- 손님 대사 & 버튼 UI ---
    public void SetGuestText(string text)
    {
        if (guestText != null) guestText.text = text;
    }

    public void SetSubmitButtonActive(bool isActive)
    {
        if (submitButton != null) submitButton.gameObject.SetActive(isActive);
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