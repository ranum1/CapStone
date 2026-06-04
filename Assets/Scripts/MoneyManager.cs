using UnityEngine;
using TMPro;
public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text goldText;

    public int Gold { get; private set; }

    private void Start()
    {
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        Gold += amount;

        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"{Gold} G";
    }

}
