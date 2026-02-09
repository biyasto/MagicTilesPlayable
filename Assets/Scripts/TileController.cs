using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer headImage;
    public SpriteRenderer tileImage;
    public Sprite defaultSprite;
    public Sprite tappedSprite;

    public bool isProcessed = false;
    public bool isTouched = false;
    [SerializeField] private BoxCollider2D tileCollider;
    [SerializeField] private SpriteRenderer highlightImage;
    private bool isTapTile;
    Camera mainCam;
    
    private bool isHolding = false;
    public float holdDuration = 0.25f;
    private float holdTimer = 0f;
    [SerializeField] private float highlightSize = 3.86f;

    private int activeFingerId = -1; // which finger is holding this tile

    void Awake()
    {
        mainCam = Camera.main;
    }

    public void Activate(System.Action returnAction)
    {
        isProcessed = false;
        isHolding = false;
        isTapTile = false;
        tileImage.sprite = defaultSprite;
        headImage.sprite = GameManager.Instance.headSprite;
        highlightImage.size = new Vector2(highlightImage.size.x, 0);

        highlightImage.gameObject.SetActive(true);

        tileImage.color = Color.white;
        tileCollider.enabled = true;
        holdTimer = 0;
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
        if( GameManager.Instance.IsGameOver) return;
        // MULTI-TOUCH (Luna / Mobile)
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector2 worldPos = mainCam.ScreenToWorldPoint(touch.position);

                // BEGIN
                if (touch.phase == TouchPhase.Began)
                {
                    TryStartHold(worldPos, touch.fingerId);
                }

                // HOLD
                if (touch.fingerId == activeFingerId &&
                    (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
                {
                    UpdateHold();
                }

                // END / CANCEL
                if (touch.fingerId == activeFingerId &&
                    (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    EndHold();
                }
            }
        }
#if UNITY_EDITOR
        // MOUSE fallback (Editor & non-touch)
        else
        {
            if (Input.GetMouseButtonDown(0))
                TryStartHold(mainCam.ScreenToWorldPoint(Input.mousePosition), 0);

            if (Input.GetMouseButton(0) && activeFingerId == 0)
                UpdateHold();

            if (Input.GetMouseButtonUp(0) && activeFingerId == 0)
                EndHold();
        }
#endif
    }

    void TryStartHold(Vector2 worldPos, int fingerId)
    {
        if (isProcessed || activeFingerId != -1) return;

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider != null && hit.collider == tileCollider)
        {
            activeFingerId = fingerId;
            isHolding = true;
            holdTimer = 0f;
            AudioManager.Instance.PlayMusic();
        }
    }

    void UpdateHold()
    {
        if (!isHolding || isProcessed) return;

        if (!isTouched)
        {
            isTouched = true;
            highlightImage.gameObject.SetActive(true);
            GameManager.Instance.AddScore(50);
        }

        holdTimer += Time.deltaTime;
        holdTimer = Mathf.Min(holdTimer, holdDuration);

        highlightImage.size = new Vector2(
            highlightImage.size.x,
            highlightSize * (holdTimer / holdDuration)
        );

        if (holdTimer >= holdDuration)
        {
            HandleTap();
        }
    }

 
    void HandleTap()
    {
        if (isProcessed) return;

        holdTimer = 0f;
        isHolding = false;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.completeSound);
        isProcessed = true;
        tileImage.sprite = tappedSprite;
        headImage.sprite = GameManager.Instance.headDeadSprite;
        headImage.flipX = Random.value > 0.5f;
        GameManager.Instance.ShakeCamera();
        GameManager.Instance.AddScore(50);
    }

    public void Reset()
    {
        isProcessed = false;
        tileImage.sprite = defaultSprite;
        tileImage.color = Color.white;
    }
    void EndHold()
    {
        isHolding = false;
        activeFingerId = -1;
    }
}