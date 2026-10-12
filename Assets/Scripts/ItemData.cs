using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Farming/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemId;       // 아이템 고유 ID
    public string itemName;     // 아이템 이름
    public Sprite itemIcon;     // UI 표시용 아이콘
}