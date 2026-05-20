using System.Runtime.CompilerServices;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    // [추가] 색상을 변경할 스프라이트 렌더러를 연결합니다.
    // 인스펙터 창에서 가마솥 안의 액체나 결과물 이미지를 드래그해서 넣어주세요.
    public SpriteRenderer resultDisplaySprite;

    public OrderManagerController OrMan;

    private ColorMixer cauldron;

    // 현재 주문 (테스트용)
    public ColorID currentOrder = ColorID.Purple;

    void Start()
    {
        cauldron = new ColorMixer();
    }

    // ... (AddIngredient 관련 코드들은 동일)

        void Update()
    {
        
    }

    public void AddingredientRed()
    {
        cauldron.AddIngredient(ColorID.Red);
        Debug.Log("마젠타 추가");
    }

    public void AddingredientBlue()
    {
        cauldron.AddIngredient(ColorID.Blue);
        Debug.Log("시안 추가");
    }

    public void AddingredientYellow()
    {
        cauldron.AddIngredient(ColorID.Yellow);
        Debug.Log("노랑 추가");
    }

    public void SubmitProductButton()
    {
        SubmitProduct();
    }

    public void ClearCauldronButton()
    {
        cauldron.ClearCauldron();
        // 초기화 시 색상을 다시 투명하게나 흰색으로 바꿀 수 있습니다.
        if (resultDisplaySprite != null) resultDisplaySprite.color = Color.white;
        Debug.Log("솥 비우기 완료");
    }

    private void SubmitProduct()
    {
        ColorProduct result = cauldron.CraftAndClear();
        Debug.Log($"제작된 상품: {result.ID} (티어: {result.Tier})");

        // [추가] ID 값에 따라 스프라이트 색상 변경
        UpdateSpriteColor(result.ID);



        if (OrMan.CheckAnswer(result.ID))
        {
            Debug.Log("주문 성공!");
            OrMan.GenerateRandomOrder();
        }
        else
        {
            Debug.Log("주문 실패...");
            OrMan.GenerateRandomOrder();
        }
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
            case ColorID.Red:
                resultDisplaySprite.color = new Color(255f, 0f, 255f);
                break;
            case ColorID.Blue:
                resultDisplaySprite.color = new Color(0f, 255f, 255f);
                break;
            case ColorID.Yellow:
                resultDisplaySprite.color = new Color(255f, 255f, 0f);
                break;
            case ColorID.Purple:
                resultDisplaySprite.color = new Color(0.5f, 0f, 0.5f); // 보라색 직접 정의
                break;
            case ColorID.Orange:
                resultDisplaySprite.color = new Color(1f, 0.5f, 0f); // 주황색
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
}