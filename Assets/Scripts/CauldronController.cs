using System.Runtime.CompilerServices;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class CauldronController : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    // [추가] 색상을 변경할 스프라이트 렌더러를 연결합니다.
    // 인스펙터 창에서 가마솥 안의 액체나 결과물 이미지를 드래그해서 넣어주세요.
    public SpriteRenderer resultDisplaySprite;

    public GameManager gameManager;
    public GuestController guestController;
    public MoneyManager moneyManager;
    public OrderManagerController OrMan;
    private OrderRecipe currentOrder;

    public TMP_Text guestText;

    private ColorMixer cauldron;
    IngredientDrag ingredientDrag;
    
    // 현재 주문 (테스트용)
    //public ColorID currentOrder = ColorID.Purple;

    void Start()
    {
        cauldron = new ColorMixer();
    }

        void Update()
    {
        
    }

    public void SubmitProductButton()
    {
        StartCoroutine(SubmitRoutine());
    }

    public void ClearCauldronButton()
    {
        cauldron.ClearCauldron();
        // 초기화 시 색상을 다시 투명하게나 흰색으로 바꿀 수 있습니다.
        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.white;
        Debug.Log("솥 비우기 완료");
    }

    public IEnumerator StartGuestSequence()
    {
        gameManager.ChangeState(GameState.Guest);

        // 첫 주문 생성
        OrMan.GenerateRandomOrder();

        // 손님 이미지 설정
        guestController.SetGuest(
            OrMan.CurrentOrder.guestSprite);

        // 아래에서 등장
        yield return StartCoroutine(
            guestController.EnterRoutine());

        // 주문 대사 출력
        guestText.text =
            OrMan.CurrentOrder.orderText;
    }

private IEnumerator SubmitRoutine()
{
    ColorProduct result = cauldron.CraftAndClear();

    UpdateSpriteColor(result.ID);

    gameManager.ChangeState(GameState.Guest);

    ClearCauldronButton();

    bool success = OrMan.CheckAnswer(result.ID);

    if (success)
    {
        guestText.text =
            OrMan.CurrentOrder.successText;

        moneyManager.AddGold(
            OrMan.CurrentOrder.reward);
    }
    else
    {
        guestText.text =
            OrMan.CurrentOrder.failText;
    }

    yield return new WaitForSeconds(3f);

    // 현재 손님 퇴장
    yield return StartCoroutine(
        guestController.ExitRoutine());

    // 손님 없는 시간
    yield return new WaitForSeconds(
        Random.Range(1f, 3f));

    // 다음 주문 생성
    OrMan.GenerateRandomOrder();

    // 다음 손님 이미지 변경
    guestController.SetGuest(
        OrMan.CurrentOrder.guestSprite);

    // 다음 손님 등장
    yield return StartCoroutine(
        guestController.EnterRoutine());
}

    // [추가] switch 문을 활용한 색상 업데이트 로직
    private void UpdateSpriteColor(ColorID id)
    {
        if (resultDisplaySprite == null)
        {
            Debug.LogWarning("resultDisplaySprite가 할당되지 않았습니다!");
            return;
        }

        switch (id)
        {
            case ColorID.Magenta:
                resultDisplaySprite.color = new Color(255f, 0f, 255f);
                break;
            case ColorID.Cyan:
                resultDisplaySprite.color = new Color(0f, 255f, 255f);
                break;
            case ColorID.Yellow:
                resultDisplaySprite.color = new Color(255f, 255f, 0f);
                break;
            case ColorID.Red:
                resultDisplaySprite.color = Color.red;
                break;
            case ColorID.Blue:
                resultDisplaySprite.color = Color.blue;
                break;
            case ColorID.Green:
                resultDisplaySprite.color = Color.green;
                break;
            case ColorID.Black:
                resultDisplaySprite.color = Color.black;
                break;
            default:
                resultDisplaySprite.color = Color.white; // 기본값
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("드롭됨");

        IngredientDrag ingredient =
            eventData.pointerDrag.GetComponent<IngredientDrag>();

        if (ingredient != null)
        {
            ingredient.isDropped = true;

            cauldron.AddIngredient(ingredient.ingredientType);

            Debug.Log(ingredient.ingredientType + " 추가");

            // Destroy(ingredient.gameObject);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 올라옴");
    }

}