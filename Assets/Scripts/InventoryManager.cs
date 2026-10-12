using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // 아이템 ID별 보유 수량 저장 (Dictionary)
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    [Header("테스트용 초기 아이템 세팅")]
    [SerializeField] private ItemData testSeedItem; // 테스트할 씨앗 ItemData
    [SerializeField] private int testSeedAmount = 1; // 테스트 수량 (1개)

    private void Start()
    {
        // 시작 시 테스트 씨앗을 설정된 수량만큼 지급
        if (testSeedItem != null)
        {
            AddItem(testSeedItem, testSeedAmount);
            Debug.Log($"[테스트] 게임 시작 시 {testSeedItem.itemName} {testSeedAmount}개 지급 완료!");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 특정 아이템의 보유 수량 확인
    public int GetItemCount(ItemData item)
    {
        if (item == null) return 0;
        return itemCounts.ContainsKey(item.itemId) ? itemCounts[item.itemId] : 0;
    }

    // 아이템 수량 추가 (수확 시 호출)
    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        if (!itemCounts.ContainsKey(item.itemId))
        {
            itemCounts[item.itemId] = 0;
        }

        itemCounts[item.itemId] += amount;
        Debug.Log($"[{item.itemName}] 수량 증가! 현재 수량: {itemCounts[item.itemId]}개");
    }

    // 아이템 수량 차감 (심을 때 호출)
    public bool ConsumeItem(ItemData item, int amount = 1)
    {
        if (item == null) return false;

        int currentAmount = GetItemCount(item);

        // 재료가 0개이거나 부족하면 차감 불가
        if (currentAmount < amount)
        {
            Debug.LogWarning($"[{item.itemName}] 재료가 부족합니다! (현재: {currentAmount}개)");
            return false;
        }

        itemCounts[item.itemId] -= amount;
        Debug.Log($"[{item.itemName}] {amount}개 사용! 남은 수량: {itemCounts[item.itemId]}개");
        return true;
    }
}