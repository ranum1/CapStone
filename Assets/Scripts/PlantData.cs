using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Data", menuName = "Farming/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public ItemData seedItem;            // 심을 때 필요한 씨앗/재료 아이템
    public ItemData harvestResultItem;   // 다 자랐을 때 수확할 결과 아이템

    public float timePerStage = 3f;      // 각 성장 단계별 소요 시간(초)
    public Sprite[] growthStages;        // 단계별 모습 (예: 0: 씨앗, 1: 새싹, 2: 다 자란 식물)

    [Header("단계별 위치 보정 (Offset)")]
    public Vector3[] stageOffsets;       // 각 단계별 PlantImage의 localPosition 값
    public int seedPrice = 10;           // 씨앗 구매 가격 (각 ScriptableObject 인스펙터에서 설정)
}