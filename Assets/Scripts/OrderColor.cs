using UnityEngine;

[CreateAssetMenu(menuName = "Game/Order Recipe")]
public class OrderRecipe : ScriptableObject
{
    public string orderText;
    public string successText;
    public string failText;

    public ColorID requiredColor;

    public int reward;

    public Sprite guestSprite;


    
}