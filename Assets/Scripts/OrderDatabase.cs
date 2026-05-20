using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Order Database")]
public class OrderDatabase : ScriptableObject
{
    public List<OrderRecipe> orders;
}