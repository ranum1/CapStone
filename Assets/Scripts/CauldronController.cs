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
        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.white;
        
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

        UpdateSpriteColor(craftedPotion.ID);
        hasCraftedPotion = true;

        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.white;

        // UI 갱신 요청
        UIManager.Instance.ResetIngredientUI();
        UIManager.Instance.SetSubmitButtonActive(true);

        gameManager.ChangeState(GameState.Guest);
        potion.SetActive(true);
        potionCon.SetPotion(craftedPotion.ID);
    }

    public IEnumerator StartGuestSequence()
    {
        gameManager.ChangeState(GameState.Guest);

        OrMan.GenerateRandomOrder();
        guestController.SetGuest(OrMan.CurrentOrder.guestSprite);

        yield return StartCoroutine(guestController.EnterRoutine());

        // UI 대사 출력 요청
        UIManager.Instance.SetGuestText(OrMan.CurrentOrder.orderText);
    }

    private IEnumerator SubmitRoutine()
    {
        if (OrMan.CheckAnswer(craftedPotion.ID))
        {
            UIManager.Instance.SetGuestText(OrMan.CurrentOrder.successText);
            moneyManager.AddGold(OrMan.CurrentOrder.reward);
        }
        else
        {
            UIManager.Instance.SetGuestText(OrMan.CurrentOrder.failText);
        }

        potion.SetActive(false);
        hasCraftedPotion = false;

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(guestController.ExitRoutine());
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        OrMan.GenerateRandomOrder();
        guestController.SetGuest(OrMan.CurrentOrder.guestSprite);

        yield return StartCoroutine(guestController.EnterRoutine());

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