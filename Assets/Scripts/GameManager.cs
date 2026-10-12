using UnityEngine;
using UnityEngine.UI;

public enum GameState { Title, Guest, Craft, Culture, Shop, Ending }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Gold { get; private set; }

    public GameState currentState;
    public GameObject title;
    public GameObject guest;
    public GameObject craft;
    public GameObject culture;
    public GameObject shop;

    [Header("Day & Ending Settings")]
    [SerializeField] private int maxDailyOrders = 7;
    private int currentOrderCount = 0;
    private int completedOrderCount = 0;

    public bool IsDayEnded => currentOrderCount >= maxDailyOrders;

    public Button cultureButton;
    public Button shopButton;
    public Button craftButton;

    public CauldronController cauldronCon;

    [SerializeField]
    private SpriteRenderer cauldronConUI;

    [Header("BGM Settings")]
    [SerializeField] private AudioSource titleBgm;
    [SerializeField] private AudioSource shopBgm;
    [SerializeField] private AudioSource inGameBgm;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Title);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        title.SetActive(false);
        guest.SetActive(false);
        craft.SetActive(false);
        culture.SetActive(false);
        shop.SetActive(false);

        switch (currentState)
        {
            case GameState.Title:
                title.SetActive(true);
                UIManager.Instance?.HideEndingPanel();
                break;

            case GameState.Guest:
                guest.SetActive(true);
                break;

            case GameState.Craft:
                craft.SetActive(true);
                break;

            case GameState.Culture:
                culture.SetActive(true);
                break;

            case GameState.Shop:
                shop.SetActive(true);
                break;

            case GameState.Ending:
                // 엔딩 상태 진입시 UIManager를 통해 엔딩 전용 검은 패널 출력
                break;
        }

        UpdateBgm();
    }

    public void RecordOrderResult(bool isSuccess)
    {
        currentOrderCount++;

        if (isSuccess) completedOrderCount++;

        int remaining = Mathf.Max(0, maxDailyOrders - currentOrderCount);
        UIManager.Instance?.UpdateRemainingOrdersUI(remaining);

        if (currentOrderCount >= maxDailyOrders)
        {
            TriggerEnding();
        }
    }

    private void TriggerEnding()
    {
        ChangeState(GameState.Ending);

        if (cauldronConUI != null) cauldronConUI.enabled = false;
        UIManager.Instance?.SetGuestTextActive(false);

        // 엔딩 성공 횟수에 따라 검은 화면 엔딩 패널 설정
        if (completedOrderCount <= 2)
        {
            UIManager.Instance?.ShowEndingPanel("영업 실패...", "오늘 손님을 너무 많이 놓쳤습니다.\n가게 문을 닫을 위기입니다.");
        }
        else if (completedOrderCount <= 6)
        {
            UIManager.Instance?.ShowEndingPanel("무난한 하루!", "오늘 하루도 무난하게 마무리되었습니다.\n내일은 더 잘해보죠!");
        }
        else // 7개 성공
        {
            UIManager.Instance?.ShowEndingPanel("영업 대성공!", "오늘 방문한 모든 손님이 크게 만족했습니다!\n최고의 물약집입니다!");
        }
    }

    // --- 엔딩 버튼 온클릭 이벤트 ---

    /// <summary>
    /// [타이틀로] 버튼 클릭 시 호출
    /// </summary>
    public void OnClickTitleButton()
    {
        currentOrderCount = 0;
        completedOrderCount = 0;
        ChangeState(GameState.Title);
    }

    /// <summary>
    /// [리트라이/다시 시작] 버튼 클릭 시 호출
    /// </summary>
    public void OnClickRetryButton()
    {
        CloseTitle();
    }

    private void UpdateBgm()
    {
        switch (currentState)
        {
            case GameState.Title:
            case GameState.Ending:
                PlayBgm(titleBgm);
                break;

            case GameState.Shop:
                PlayBgm(shopBgm);
                break;

            case GameState.Guest:
            case GameState.Craft:
            case GameState.Culture:
                PlayBgm(inGameBgm);
                break;
        }
    }

    private void PlayBgm(AudioSource targetBgm)
    {
        if (targetBgm != null && targetBgm.isPlaying) return;

        if (titleBgm != null) titleBgm.Stop();
        if (shopBgm != null) shopBgm.Stop();
        if (inGameBgm != null) inGameBgm.Stop();

        if (targetBgm != null) targetBgm.Play();
    }

    public void GuestButton()
    {
        ChangeState(GameState.Guest);
        if (cauldronConUI != null) cauldronConUI.enabled = false;
    }

    public void CraftButton()
    {
        ChangeState(GameState.Craft);
        if (cauldronConUI != null) cauldronConUI.enabled = true;
    }

    public void CultureButton()
    {
        ChangeState(GameState.Culture);
        if (cauldronConUI != null) cauldronConUI.enabled = false;
    }

    public void ShopButton()
    {
        ChangeState(GameState.Shop);
        if (cauldronConUI != null) cauldronConUI.enabled = false;
    }

    public void CloseTitle()
    {
        currentOrderCount = 0;
        completedOrderCount = 0;

        UIManager.Instance?.UpdateRemainingOrdersUI(maxDailyOrders);
        UIManager.Instance?.HideEndingPanel();

        if (cauldronCon != null && !cauldronCon.gameObject.activeSelf)
        {
            cauldronCon.gameObject.SetActive(true);
        }

        StartCoroutine(cauldronCon.StartGuestSequence());
        ChangeState(GameState.Guest);
    }
}