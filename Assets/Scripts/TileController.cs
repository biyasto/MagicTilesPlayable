using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer tileImage;
    public Sprite defaultSprite;
    public Sprite tappedSprite;

    private bool isProcessed = false;
    [SerializeField] private BoxCollider2D tileCollider;

    Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    public void Activate(System.Action returnAction)
    {
        isProcessed = false;
        tileImage.sprite = defaultSprite;
        tileImage.color = Color.white;
        tileCollider.enabled = true;
    }

    public void MoveDown(float speed, float deltaTime)
    {
        transform.Translate(Vector3.down * speed * deltaTime);
    }

    public bool IsOffScreen()
    {
        return transform.position.y <= -6f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick(Input.mousePosition);
        }
    }

    void CheckClick(Vector2 screenPos)
    {
        if (isProcessed) return;

        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider == tileCollider)
        {
            HandleTap();
        }
    }

    void HandleTap()
    {
        isProcessed = true;
        tileImage.sprite = tappedSprite;
        tileImage.color = new Color(1f, 0.98f, 0f);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.tapSound);
    }

    public void Reset()
    {
        isProcessed = false;
        tileImage.sprite = defaultSprite;
        tileImage.color = Color.white;
    }
}