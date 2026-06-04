using UnityEngine;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;

public class OrderManagerController : MonoBehaviour
{
    public OrderDatabase database;

    public TMP_Text orderTextUI;
    

    private OrderRecipe currentOrder;

    public OrderRecipe CurrentOrder => currentOrder;

    void Start()
    {
        GenerateRandomOrder();
    }


    public void GenerateRandomOrder()
    {
        int randomIndex =
            Random.Range(0, database.orders.Count);

        currentOrder = database.orders[randomIndex];

        orderTextUI.text = currentOrder.orderText;
        

        Debug.Log("정답 색상: " +
                    currentOrder.requiredColor);
    }

        public bool CheckAnswer(ColorID playerColor)
        {
            return playerColor ==
                currentOrder.requiredColor;
        }
}