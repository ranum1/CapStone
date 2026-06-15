using System.Collections;
using UnityEngine;

public class GuestController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer guestRenderer;

    [SerializeField]
    private float moveDistance = 3f;

    [SerializeField]
    private float moveDuration = 0.5f;

    private Vector3 idlePosition;

    private void Awake()
    {
        // 손님이 평소 서 있을 위치 저장
        idlePosition = transform.position;
    }

    public void SetGuest(Sprite sprite)
    {
        guestRenderer.sprite = sprite;
    }

    public IEnumerator EnterRoutine()
    {
        Vector3 startPos =
            idlePosition + Vector3.down * moveDistance;

        transform.position = startPos;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position =
                Vector3.Lerp(startPos,
                            idlePosition,
                            t);

            yield return null;
        }

        transform.position = idlePosition;
    }

    public IEnumerator ExitRoutine()
    {
        Vector3 endPos =
            idlePosition + Vector3.down * moveDistance;

        Vector3 startPos = transform.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position =
                Vector3.Lerp(startPos,
                            endPos,
                            t);

            yield return null;
        }

        transform.position = endPos;
    }
}