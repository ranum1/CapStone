using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CauldronController : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [Header("가마솥 비주얼")]
    public SpriteRenderer resultDisplaySprite;

    [Header("매니저 참조")]
    public GameManager gameManager;
    public GuestController guestController;
    public MoneyManager moneyManager;
    public OrderManagerController OrMan;
    public PotionController potionCon;
    public GameObject potion;

    private ColorMixer cauldron;
    private ColorProduct craftedPotion;
    private bool hasCraftedPotion = false;

    [SerializeField]
    private AudioSource dripSound;

    void Start()
    {
        cauldron = new ColorMixer();
    }

    public void SubmitPotion()
    {
        if (!hasCraftedPotion)
        {
            Debug.Log("제출할 물약이 없습니다.");
            return;
        }

        StartCoroutine(SubmitRoutine());
        UIManager.Instance.SetSubmitButtonActive(false);
    }

    public void ClearCauldronButton()
    {
        cauldron.ClearCauldron();
        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.gray;
        
        // UI 갱신 요청
        UIManager.Instance.ResetIngredientUI();
        Debug.Log("솥 비우기 완료");
    }

    public void CraftPotionButton()
    {
        CraftPotion();
    }

    private void CraftPotion()
    {
        craftedPotion = cauldron.CraftAndClear();

        Debug.Log($"제작된 색상 ID : {craftedPotion.ID}");

        hasCraftedPotion = true;

        // 가마솥 UI 및 비주얼 리셋
        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.white;
        UIManager.Instance?.ResetIngredientUI();
        UIManager.Instance?.SetSubmitButtonActive(true);

        // 손님 화면으로 전환
        gameManager.ChangeState(GameState.Guest);

        // [체크!] PotionController 연결 확인 및 실행
        if (potionCon != null)
        {
            // SetPotion 내부에서 자기 자신(Potion GameObject) 및 자식(Liquid, Bottle)을 모두 켭니다.
            potionCon.SetPotion(craftedPotion.ID); 
        }
        else
        {
            Debug.LogError("[에러] CauldronController의 PotionCon 필드가 비어있습니다! Inspector에서 Potion 오브젝트를 연결해 주세요.");
        }
    }

    private IEnumerator SubmitRoutine()
    {
        bool isSuccess = OrMan.CheckAnswer(craftedPotion.ID);

        if (isSuccess)
        {
            UIManager.Instance.SetGuestText(OrMan.CurrentOrder.successText);
            moneyManager.AddGold(OrMan.CurrentOrder.reward);
        }
        else
        {
            UIManager.Instance.SetGuestText(OrMan.CurrentOrder.failText);
        }

        GameManager.Instance.RecordOrderResult(isSuccess);

        // [핵심] PotionController를 통해 포션과 자식(Bottle/Liquid) 오브젝트를 함께 끕니다.
        if (potionCon != null)
        {
            potionCon.HidePotion();
        }
        else if (potion != null)
        {
            potion.SetActive(false);
        }

        hasCraftedPotion = false;

        yield return new WaitForSeconds(2.5f);

        // 손님 퇴장 시 대화창 비활성화
        UIManager.Instance.SetGuestTextActive(false);

        yield return StartCoroutine(guestController.ExitRoutine());

        // 7번째 주문 완료 시 하루 종료
        if (GameManager.Instance.IsDayEnded)
        {
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        // 다음 손님 생성 및 등장
        OrMan.GenerateRandomOrder();
        guestController.SetGuest(OrMan.CurrentOrder.guestSprite);

        yield return StartCoroutine(guestController.EnterRoutine());

        UIManager.Instance.SetGuestTextActive(true);
        UIManager.Instance.SetGuestText(OrMan.CurrentOrder.orderText);
    }

    public IEnumerator StartGuestSequence()
    {
        gameManager.ChangeState(GameState.Guest);

        OrMan.GenerateRandomOrder();
        guestController.SetGuest(OrMan.CurrentOrder.guestSprite);

        // 시작 시 일단 대화창 끄고 등장
        UIManager.Instance.SetGuestTextActive(false);

        yield return StartCoroutine(guestController.EnterRoutine());

        // 등장 완료 후 대화창 켜고 대사 출력
        UIManager.Instance.SetGuestTextActive(true);
        UIManager.Instance.SetGuestText(OrMan.CurrentOrder.orderText);
    }

    private void UpdateSpriteColor(ColorID id)
    {
        if (resultDisplaySprite == null) return;
        resultDisplaySprite.color = ColorBlendTable.GetBlendedColor(id);
    }

    public void OnDrop(PointerEventData eventData)
    {
        IngredientDrag ingredient = eventData.pointerDrag.GetComponent<IngredientDrag>();
        dripSound.Play();
        if (ingredient != null)
        {
            bool isAdded = cauldron.AddIngredient(ingredient.ingredientType);

            if (isAdded)
            {
                ingredient.isDropped = true;

                // 솥 안의 액체 색상 변경
                UpdateSpriteColor(cauldron.CurrentMixture);

                // UI 재료 슬롯 업데이트 요청
                UIManager.Instance.UpdateIngredientUI(cauldron.CurrentMixture);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { }
}