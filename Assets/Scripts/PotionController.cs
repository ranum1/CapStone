using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private ColorID potionColor;
    public ColorID PotionColor => potionColor;

    /// <summary>
    /// ColorBlendTable을 활용하여 포션의 색상을 업데이트합니다.
    /// </summary>
    public void SetPotion(ColorID color)
    {
        potionColor = color;

        if (spriteRenderer != null)
        {
            // Sprite를 교체하는 대신 ColorBlendTable에서 RGB 값을 가져와 색상을 변경합니다.
            spriteRenderer.color = ColorBlendTable.GetBlendedColor(color);
        }

        Debug.Log($"포션 생성 완료 - ColorID: {color}");

        // 물약 오브젝트 활성화
        gameObject.SetActive(true);
    }
}