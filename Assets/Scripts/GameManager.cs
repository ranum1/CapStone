using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState { Title, Guest, Craft, Culture, Shop }

public class GameManager : MonoBehaviour
{
    public int Gold { get; private set; }

    public GameState currentState;
    public GameObject title;
    public GameObject guest;
    public GameObject craft;
    public GameObject culture;
    public GameObject shop;

    public Button cultureButton;
    public Button shopButton;
    public Button craftButton;

    public CauldronController cauldronCon;


    private void Start()
    {
        ChangeState(GameState.Title);
    }

        private void Update()
    {
        switch (currentState)
        {
            case GameState.Title:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(cauldronCon.StartGuestSequence());
                }
                break;

            case GameState.Guest:
                    ChangeState(GameState.Guest);
                break;

            case GameState.Culture:
                    ChangeState(GameState.Culture);
                break;
            
            case GameState.Shop:
                    ChangeState(GameState.Shop);
                break;

            }
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
        }
    }

    public void GuestButton()
    {
        ChangeState(GameState.Guest);
    }

    public void CraftButton()
    {
        ChangeState(GameState.Craft);
    }

    public void CultureButton()
    {
        ChangeState(GameState.Culture);
    }

    public void ShopButton()
    {
        ChangeState(GameState.Shop);
    }
}

