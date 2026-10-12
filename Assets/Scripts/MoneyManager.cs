using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; } // 다른 스크립트에서 접근하기 위한 싱글톤

    [SerializeField]
    private TMP_Text goldText;

    public int Gold { get; private set; } = 20; // 초기 골드 20으로 시작!

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateGoldUI();
    }

    // 골드 추가 (수확/판매 시)
    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateGoldUI();
    }

    // 골드가 충분한지 확인하는 함수
    public bool HasEnoughGold(int amount)
    {
        return Gold >= amount;
    }

    // 골드 차감 시도 함수 (구매 성공 시 true 반환)
    public bool TrySpendGold(int amount)
    {
        if (HasEnoughGold(amount))
        {
            Gold -= amount;
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"{Gold} G";
        }
    }
}