using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [Header("상품 데이터")]
    public PlantData plantDataInSlot; // 인스펙터에서 해당 상품(씨앗) 데이터 연결

    private Button slotButton;

    private void Awake()
    {
        // 슬롯(패널) 클릭 감지용 Button 컴포넌트 연결
        slotButton = GetComponent<Button>();
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }

    private void OnSlotClicked()
    {
        if (plantDataInSlot == null || plantDataInSlot.seedItem == null)
        {
            Debug.LogWarning("슬롯에 PlantData 또는 SeedItem이 설정되지 않았습니다.");
            return;
        }

        // 상점 매니저에게 클릭한 상품 데이터를 전달하며 팝업을 열어줌
        ShopManager.Instance.OpenBuyConfirmPopup(plantDataInSlot);
    }
}