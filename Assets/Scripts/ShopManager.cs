using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용 시 (일반 Text라면 UnityEngine.UI.Text 사용)

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("미리 만들어둔 구매 팝업 패널")]
    [SerializeField] private GameObject buyConfirmPopupPanel; // 구매 확인 팝업 패널

    [Header("상점 주인 대사 연동")]
    [SerializeField] private TextMeshProUGUI ownerText;       // 주인의 말풍선/문구 텍스트 (구매하시겠습니까?)

    [Header("팝업창 내부 단일 텍스트 상자")]
    [SerializeField] private TextMeshProUGUI popupInfoText;    // 씨앗 이름, 가격, 보유 수량이 들어가는 텍스트

    [Header("버튼 연결")]
    [SerializeField] private Button buyButton;                // "구매하기" 버튼
    [SerializeField] private Button noButton;                 // "아니오" 버튼

    [Header("상점 주인 대사 설정")]
    [TextArea]
    [SerializeField] private string ownerDefaultText = "어떤 씨앗을 구매하실건가요?"; // 기본 대사

    private PlantData currentSelectedPlantData;               // 현재 선택된 상품
    private Coroutine textResetCoroutine;                      // 문구 원복 코루틴 참조용

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(CloseBuyConfirmPopup);
        }

        if (buyConfirmPopupPanel != null)
        {
            buyConfirmPopupPanel.SetActive(false);
        }

        SetOwnerText(ownerDefaultText);
    }

    private void OnEnable()
    {
        ResetShopState();
    }

    public void ResetShopState()
    {
        if (textResetCoroutine != null)
        {
            StopCoroutine(textResetCoroutine);
            textResetCoroutine = null;
        }

        SetOwnerText(ownerDefaultText);

        currentSelectedPlantData = null;
        if (buyConfirmPopupPanel != null)
        {
            buyConfirmPopupPanel.SetActive(false);
        }
    }

    // 상품 클릭 시 팝업 열기 (ShopSlotUI에서 호출)
    public void OpenBuyConfirmPopup(PlantData plantData)
    {
        currentSelectedPlantData = plantData;

        if (plantData == null) return;

        // 진행 중이던 구매 완료 타이머(2초)가 있다면 취소
        if (textResetCoroutine != null)
        {
            StopCoroutine(textResetCoroutine);
            textResetCoroutine = null;
        }

        // 1. 씨앗 정보 가져오기
        string itemName = (plantData.seedItem != null && !string.IsNullOrEmpty(plantData.seedItem.itemName))
            ? plantData.seedItem.itemName
            : plantData.plantName;

        int price = plantData.seedPrice;

        // 2. 현재 내 인벤토리에 있는 이 씨앗의 보유 수량 가져오기
        int currentOwnCount = 0;
        if (InventoryManager.Instance != null && plantData.seedItem != null)
        {
            currentOwnCount = InventoryManager.Instance.GetItemCount(plantData.seedItem);
        }

        // 3. 주인의 말풍선에는 질문 대사 세팅
        SetOwnerText($"{itemName}을(를) 구매하시겠습니까?");

        // 4. 하나의 텍스트 상자에 [이름], [가격], [현재 보유 수량]을 예쁘게 출력
        if (popupInfoText != null)
        {
            popupInfoText.text = $"<b>{itemName}</b>\n가격: {price} G\n현재 보유 수량: {currentOwnCount}개";
        }

        // 5. 팝업 패널 활성화
        if (buyConfirmPopupPanel != null)
        {
            buyConfirmPopupPanel.SetActive(true);
        }
    }

    // "구매하기" 버튼 클릭 시 실행
    private void OnBuyButtonClicked()
    {
        if (currentSelectedPlantData == null || currentSelectedPlantData.seedItem == null) return;

        int price = currentSelectedPlantData.seedPrice;

        // 골드 확인 및 차감 시도
        if (MoneyManager.Instance != null && MoneyManager.Instance.TrySpendGold(price))
        {
            // [구매 성공]
            InventoryManager.Instance.AddItem(currentSelectedPlantData.seedItem, 1);

            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.RefreshAllSlots();
            }

            Debug.Log($"[{currentSelectedPlantData.seedItem.itemName}] 구매 완료! ({price}G 차감)");

            // 팝업창 즉시 닫기
            if (buyConfirmPopupPanel != null)
            {
                buyConfirmPopupPanel.SetActive(false);
            }
            currentSelectedPlantData = null;

            // 주인 대사를 2초간 "구매 완료되었습니다!"로 표시 후 원복
            if (textResetCoroutine != null) StopCoroutine(textResetCoroutine);
            textResetCoroutine = StartCoroutine(ShowMessageRoutine("구매 완료되었습니다!"));
        }
        else
        {
            // [골드 부족 실패]
            Debug.Log("골드가 부족하여 씨앗을 구매할 수 없습니다.");

            // 팝업창 즉시 닫기
            if (buyConfirmPopupPanel != null)
            {
                buyConfirmPopupPanel.SetActive(false);
            }
            currentSelectedPlantData = null;

            // 주인 대사를 2초간 "골드가 부족합니다!"로 표시 후 원복
            if (textResetCoroutine != null) StopCoroutine(textResetCoroutine);
            textResetCoroutine = StartCoroutine(ShowMessageRoutine("골드가 부족합니다!"));
        }
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        SetOwnerText(message);

        yield return new WaitForSeconds(2.0f);

        SetOwnerText(ownerDefaultText);
        textResetCoroutine = null;
    }

    // "아니오" 버튼 클릭 또는 팝업창 닫기
    public void CloseBuyConfirmPopup()
    {
        currentSelectedPlantData = null;

        if (textResetCoroutine != null)
        {
            StopCoroutine(textResetCoroutine);
            textResetCoroutine = null;
        }

        SetOwnerText(ownerDefaultText);

        if (buyConfirmPopupPanel != null)
        {
            buyConfirmPopupPanel.SetActive(false);
        }
    }

    private void SetOwnerText(string text)
    {
        if (ownerText != null)
        {
            ownerText.text = text;
        }
    }
}