using UnityEngine;

public class MovePingPong : MonoBehaviour
{
    public Vector3 offset = new Vector3(50f, 0f, 0f);
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private bool isMoving = false;
    private float startTime;

    private RectTransform rectTransform;
    private Transform cachedTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cachedTransform = transform;

        // Save start position correctly
        startPos = rectTransform != null
            ? rectTransform.anchoredPosition3D
            : cachedTransform.localPosition;
    }

    void Update()
    {
        if (!isMoving) return;

        float t = (Mathf.Sin((Time.time - startTime) * moveSpeed) + 1f) / 2f;
        Vector3 target = startPos + offset;

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, target, t);
        }
        else
        {
            cachedTransform.localPosition = Vector3.Lerp(startPos, target, t);
        }
    }

    public void StartMove()
    {
        startTime = Time.time;
        isMoving = true;
    }
}