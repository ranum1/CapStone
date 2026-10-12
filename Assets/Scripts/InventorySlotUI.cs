using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우 추가

public class InventorySlotUI : MonoBehaviour
{
    [Header("슬롯 설정")]
    public PlantData plantDataInSlot; // 이 슬롯이 담당하는 식물/씨앗 데이터

    [Header("UI 연동")]
    [SerializeField] private TextMeshProUGUI countText; // TextMeshPro 사용 시
    // [SerializeField] private Text countText;       // 일반 legacy Text 사용 시

    private Button slotButton;

    private void Awake()
    {
        slotButton = GetComponent<Button>();
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }

    private void OnEnable()
    {
        // 인벤토리 창이 열릴 때마다 수량 갱신
        UpdateSlotUI();
    }

    // 슬롯 UI 텍스트 갱신 함수
    public void UpdateSlotUI()
    {
        if (plantDataInSlot != null && plantDataInSlot.seedItem != null)
        {
            // InventoryManager에서 해당 씨앗의 보유 수량을 가져옴
            int currentCount = InventoryManager.Instance.GetItemCount(plantDataInSlot.seedItem);

            if (countText != null)
            {
                // 숫자를 두 자리 형태(01, 02...)로 표시하려면 "D2", 일반 표시는 ToString()
                countText.text = currentCount.ToString("D2");
            }
        }
        else
        {
            if (countText != null) countText.text = "00";
        }
    }

    // 슬롯 클릭 이벤트
    private void OnSlotClicked()
    {
        if (plantDataInSlot == null) return;

        PlantPot targetPot = InventoryUI.Instance.TargetPot;

        if (targetPot != null)
        {
            // 씨앗 심기 시도
            bool isPlanted = targetPot.Plant(plantDataInSlot);

            if (isPlanted)
            {
                // 심기에 성공했으면 수량이 변경되었으므로 UI 갱신 후 창 닫기
                UpdateSlotUI();
                InventoryUI.Instance.CloseInventory();
            }
        }
    }
}