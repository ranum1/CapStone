using UnityEngine;

public class RecipeBookController : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite hoverSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject recipePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && recipePanel.activeSelf == true)
        {
            recipePanel.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnMouseDown()
    {
        recipePanel.SetActive(true);
    }
}