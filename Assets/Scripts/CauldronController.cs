using UnityEngine;

// MonoBehaviour를 상속받아 유니티 엔진과 상호작용합니다.
public class CauldronController : MonoBehaviour
{
    // 로직을 담당할 믹서기 객체를 멤버 변수로 가집니다.
    private ColorMixer cauldron;

    // 현재 주문 (테스트용)
    public ColorID currentOrder = ColorID.Purple;

    void Start()
    {
        // 게임 시작 시 믹서기 인스턴스화
        cauldron = new ColorMixer();
    }

    void Update()
    {
        
    }

    public void AddingredientRed()
    {
        cauldron.AddIngredient(ColorID.Red);
        Debug.Log("빨간색 추가");
        
    }

    public void AddingredientBlue()
    {
        cauldron.AddIngredient(ColorID.Blue);
        Debug.Log("파란색 추가");
    }

    public void AddingredientYellow()
    {
        cauldron.AddIngredient(ColorID.Yellow);
        Debug.Log("노란색 추가");
    }

    public void SubmitProductButton()
    {
        SubmitProduct();
        
    }

    public void ClearCauldronButton()
    {
        cauldron.ClearCauldron();
        Debug.Log("색 지워짐");
    }

    private void SubmitProduct()
    {
        ColorProduct result = cauldron.CraftAndClear();
        Debug.Log($"제출된 상품: {result.ID} (티어: {result.Tier})");

        if (result.ID == currentOrder)
        {
            Debug.Log("정답");
        }
        else
        {
            Debug.Log("오답");
        }
    }
}
