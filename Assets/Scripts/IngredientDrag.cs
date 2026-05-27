using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ColorID ingredientType;
    
    public bool isDropped = false;

    ColorMixer cauldron;
    Collider2D col;

    Vector3 startPos;



    void Start()
{
    col = GetComponent<Collider2D>();
}

    public void OnBeginDrag(PointerEventData eventData)
    {
        col.enabled = false;
        startPos = transform.position;
        isDropped = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos =
            Camera.main.ScreenToWorldPoint(eventData.position);

        mousePos.z = 0;

        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        col.enabled = true;
        
        transform.position = startPos;

    }
}
