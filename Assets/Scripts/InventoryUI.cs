using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("UI 연결")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] allSlots; // 인벤토리 슬롯들

    public PlantPot TargetPot { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    // 화분을 눌러 인벤토리 창을 열 때 호출
    public void OpenInventoryForPot(PlantPot pot)
    {
        TargetPot = pot;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            RefreshAllSlots(); // 창이 열릴 때 모든 슬롯 수량 최신화
        }
    }

    // 모든 슬롯의 수량 UI 갱신
    public void RefreshAllSlots()
    {
        if (allSlots == null || allSlots.Length == 0)
        {
            allSlots = GetComponentsInChildren<InventorySlotUI>(true);
        }

        foreach (var slot in allSlots)
        {
            slot.UpdateSlotUI();
        }
    }

    public void CloseInventory()
    {
        TargetPot = null;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }
}