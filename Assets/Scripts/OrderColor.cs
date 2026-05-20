using UnityEngine;

[CreateAssetMenu(menuName = "Game/Order Recipe")]
public class OrderRecipe : ScriptableObject
{
    public string orderText;

    public ColorID requiredColor;

    public int reward;
}