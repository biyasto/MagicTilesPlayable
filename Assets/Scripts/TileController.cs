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

    void Awake()
    {
        mainCam = Camera.main;
    }

    public void Activate(System.Action returnAction)
    {
        isProcessed = false;
        isHolding = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            TryStartHold(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            if(isTouched==false)
            {
                isTouched = true;
                highlightImage.gameObject.SetActive(true);
            }
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                isHolding = false;
                holdTimer = holdDuration;
            }

            highlightImage.size = new Vector2(highlightImage.size.x, highlightSize*holdTimer*1.0f/holdDuration);

            if (holdTimer >= holdDuration)
            {   
                HandleTap();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false; // NOTICE: timer is NOT reset
        }
    }    
    void TryStartHold(Vector2 screenPos)
    {
        if (isProcessed) return;

        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider == tileCollider)
        {
            isHolding = true;
            AudioManager.Instance.PlayMusic();
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
    }

    public void Reset()
    {
        isProcessed = false;
        tileImage.sprite = defaultSprite;
        tileImage.color = Color.white;
    }
}