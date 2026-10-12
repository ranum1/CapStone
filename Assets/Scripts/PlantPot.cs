using System;
using UnityEngine;

public enum PotState
{
    Empty,          // 비어있는 상태
    Growing,        // 자라는 중
    ReadyToHarvest  // 다 자란 상태 (수확 가능)
}

public class PlantPot : MonoBehaviour
{
    [Header("화분 상태")]
    public PotState currentState = PotState.Empty;
    public PlantData currentPlantData;

    [Header("시각 연동")]
    [SerializeField] private SpriteRenderer plantSpriteRenderer;
    [SerializeField] private GameObject timerObject; // 자라는 동안 켜질 타이머 UI/오브젝트

    private int currentStage = 0;
    private float timer = 0f;

    private DateTime disableTime; // 오브젝트가 꺼진 시각 저장용 변수

    private void Start()
    {
        // 게임 시작 시 타이머 표시 상태 초기화
        UpdateTimerVisibility();
    }

    // 화분 오브젝트(또는 메뉴)가 꺼질 때 호출
    private void OnDisable()
    {
        // 자라는 중일 때만 꺼진 시각을 기록
        if (currentState == PotState.Growing)
        {
            disableTime = DateTime.Now;
        }
    }

    // 화분 오브젝트(또는 메뉴)가 다시 켜질 때 호출
    private void OnEnable()
    {
        // 자라는 중이었고, 데이터가 정상적으로 있다면 지나간 시간 계산
        if (currentState == PotState.Growing && currentPlantData != null)
        {
            TimeSpan elapsedTime = DateTime.Now - disableTime;
            float offlineSeconds = (float)elapsedTime.TotalSeconds;

            if (offlineSeconds > 0)
            {
                Debug.Log($"메뉴에 있던 동안 {offlineSeconds:F1}초가 지났습니다. 성장을 반영합니다.");
                SimulateOfflineGrowth(offlineSeconds);
            }
        }

        // 켜질 때 타이머 UI 가시성 최신화
        UpdateTimerVisibility();
    }

    // 꺼져있던 시간만큼 한 번에 자라나게 하는 로직
    private void SimulateOfflineGrowth(float passedSeconds)
    {
        timer += passedSeconds;

        while (timer >= currentPlantData.timePerStage && currentState == PotState.Growing)
        {
            timer -= currentPlantData.timePerStage;
            currentStage++;

            if (currentPlantData.growthStages != null &&
                currentStage >= currentPlantData.growthStages.Length - 1)
            {
                currentState = PotState.ReadyToHarvest;
                timer = 0f;
                Debug.Log($"[{currentPlantData.plantName}] 오프라인 동안 다 자랐습니다!");
                break;
            }
        }

        UpdateSprite();
        UpdateTimerVisibility(); // 상태 변경에 따른 타이머 UI ON/OFF
    }

    private void FixedUpdate()
    {
        if (currentState == PotState.Growing)
        {
            ProcessGrowth();
        }
    }

    // 화분 클릭 감지 (OnMouseDown)
    private void OnMouseDown()
    {
        if (currentState == PotState.ReadyToHarvest)
        {
            Harvest();
        }
        else if (currentState == PotState.Empty)
        {
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.OpenInventoryForPot(this);
            }
            else
            {
                Debug.LogError("InventoryUI Instance가 씬에 존재하지 않습니다!");
            }
        }
        else if (currentState == PotState.Growing)
        {
            Debug.Log("식물이 아직 자라는 중입니다.");
        }
    }

    // 1. 씨앗 심기 (성공 여부를 bool로 반환)
    public bool Plant(PlantData plantData)
    {
        if (plantData == null)
        {
            Debug.LogError("[Plant] 실패: 전달된 PlantData가 null입니다.");
            return false;
        }

        if (currentState != PotState.Empty)
        {
            Debug.Log("[Plant] 실패: 화분이 비어있지 않음");
            return false;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("[Plant] 실패: InventoryManager.Instance가 씬에 없습니다.");
            return false;
        }

        if (!InventoryManager.Instance.ConsumeItem(plantData.seedItem, 1))
        {
            Debug.Log("재료가 0개이므로 심을 수 없습니다.");
            return false;
        }

        currentPlantData = plantData;
        currentState = PotState.Growing;
        currentStage = 0;
        timer = 0f;

        UpdateSprite();
        UpdateTimerVisibility(); // 타이머 UI 활성화 (Growing)
        Debug.Log($"[{currentPlantData.plantName}] 씨앗을 화분에 심었습니다! (성장 시작)");
        return true;
    }

    // 2. 시간 경과 성장
    private void ProcessGrowth()
    {
        if (currentPlantData == null)
        {
            Debug.LogError("currentPlantData가 Null입니다! 성장 중단!");
            currentState = PotState.Empty;
            UpdateTimerVisibility();
            return;
        }

        // FixedUpdate 주기에 맞추어 fixedDeltaTime 사용
        timer += Time.fixedDeltaTime;

        if (timer >= currentPlantData.timePerStage)
        {
            timer = 0f;
            currentStage++;

            UpdateSprite();

            if (currentPlantData.growthStages == null || currentPlantData.growthStages.Length == 0)
            {
                Debug.LogError($"[{currentPlantData.plantName}] GrowthStages 배열이 비어있습니다!");
                return;
            }

            if (currentStage >= currentPlantData.growthStages.Length - 1)
            {
                currentState = PotState.ReadyToHarvest;
                UpdateTimerVisibility(); // 타이머 UI 비활성화 (ReadyToHarvest)
                Debug.Log($"[{currentPlantData.plantName}] 다 자랐습니다! 클릭해서 수확하세요.");
            }
            else
            {
                Debug.Log($"[{currentPlantData.plantName}] {currentStage + 1}단계로 성장했습니다!");
            }
        }
    }

    // 3. 수확
    private void Harvest()
    {
        if (currentPlantData != null && currentPlantData.harvestResultItem != null)
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(currentPlantData.harvestResultItem, 1);
            }
        }

        currentState = PotState.Empty;
        currentPlantData = null;
        currentStage = 0;
        timer = 0f;

        if (plantSpriteRenderer != null)
        {
            plantSpriteRenderer.sprite = null;
        }

        UpdateTimerVisibility(); // 타이머 UI 비활성화 (Empty)
        Debug.Log("수확이 완료되어 화분이 비었습니다.");
    }

    // 4. 식물 스프라이트 갱신
    private void UpdateSprite()
    {
        if (plantSpriteRenderer == null) return;

        if (currentPlantData != null &&
            currentPlantData.growthStages != null &&
            currentStage < currentPlantData.growthStages.Length)
        {
            plantSpriteRenderer.sprite = currentPlantData.growthStages[currentStage];

            if (currentPlantData.stageOffsets != null && currentStage < currentPlantData.stageOffsets.Length)
            {
                plantSpriteRenderer.transform.localPosition = currentPlantData.stageOffsets[currentStage];
            }
            else
            {
                plantSpriteRenderer.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            plantSpriteRenderer.sprite = null;
        }
    }

    // 5. 타이머 UI/오브젝트 표시 여부 제어
    private void UpdateTimerVisibility()
    {
        if (timerObject != null)
        {
            // 식물이 자라는 중(Growing) 상태일 때만 타이머를 켬
            bool isGrowing = (currentState == PotState.Growing);
            timerObject.SetActive(isGrowing);
        }
    }
}