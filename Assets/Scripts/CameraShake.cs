using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;

    private Vector3 startPos;
    private float timer;
    private bool isShaking;

    void Awake()
    {
        // lightweight singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        startPos = transform.localPosition;
    }

    void Update()
    {
        if (!isShaking) return;

        timer += Time.deltaTime;

        if (timer >= shakeDuration)
        {
            isShaking = false;
            transform.localPosition = startPos;
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;
        transform.localPosition = startPos + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }

    [ContextMenu("Shake Camera")]
    // Call this to trigger shake
    public void Shake()
    {
        timer = 0f;
        isShaking = true;
    }
}