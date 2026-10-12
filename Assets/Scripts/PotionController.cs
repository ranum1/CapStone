using UnityEngine;
using UnityEngine.UI;

public class PotionController : MonoBehaviour
{
    [Header("2D SpriteRenderer 방식인 경우")]
    [SerializeField] private SpriteRenderer bottleRenderer;
    [SerializeField] private SpriteRenderer liquidRenderer;

    private ColorID potionColor;
    public ColorID PotionColor => potionColor;

    /// <summary>
    /// 포션의 색상을 설정하고 빈 병 및 내용물 오브젝트를 모두 활성화합니다.
    /// </summary>
    public void SetPotion(ColorID color)
    {
        potionColor = color;
        Color targetColor = ColorBlendTable.GetBlendedColor(color);

        gameObject.SetActive(true);

        // 1. SpriteRenderer 방식
        if (liquidRenderer != null)
        {
            liquidRenderer.gameObject.SetActive(true);
            liquidRenderer.color = targetColor;
        }

        if (bottleRenderer != null)
        {
            bottleRenderer.gameObject.SetActive(true);
        }

        Debug.Log($"[PotionController] 포션 생성 완료! 색상: {targetColor}");
    }

    /// <summary>
    /// 제출 완료 시 부모 포션 오브젝트와 자식(Bottle, Liquid) 오브젝트를 모두 비활성화합니다.
    /// </summary>
    public void HidePotion()
    {
        if (liquidRenderer != null) liquidRenderer.gameObject.SetActive(false);
        if (bottleRenderer != null) bottleRenderer.gameObject.SetActive(false);


        gameObject.SetActive(false);
        Debug.Log("[PotionController] 포션 및 자식 오브젝트 비활성화 완료");
    }
}