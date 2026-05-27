using UnityEngine;
using UnityEngine.UI;

public enum GameState { Title, Guest, Craft }

public class GameManager : MonoBehaviour
{

    public GameState currentState;
    public GameObject title;
    public GameObject guest;
    public GameObject craft;

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
                    ChangeState(GameState.Guest);
                }
                break;

            case GameState.Guest:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ChangeState(GameState.Craft);
                }
                break;

            }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        title.SetActive(false);
        guest.SetActive(false);
        craft.SetActive(false);

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
        }
    }
}

