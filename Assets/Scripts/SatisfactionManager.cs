using TMPro;
using UnityEngine;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text goldText;

    public int Satisfaction { get; private set; }

    private void Start()
    {
        
    }

    public void AddSatisfaction(int amount)
    {
        Satisfaction += amount;
    }
}
