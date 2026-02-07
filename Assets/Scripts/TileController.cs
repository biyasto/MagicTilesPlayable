using UnityEngine;

public class TileController : MonoBehaviour
{
    public SpriteRenderer tileImage;
    public Sprite defaultSprite;
    public Sprite tappedSprite;

    private System.Action onReturn;
    private bool isProcessed = false;
    [SerializeField] private BoxCollider2D tileCollider;

   

    public void Activate(System.Action returnAction) {
        onReturn = returnAction;
        isProcessed = false;
        tileImage.sprite = defaultSprite;
        tileImage.color = Color.white;
        tileCollider.enabled = true;
    }

    public void MoveDown(float speed, float deltaTime) {
        transform.Translate(Vector3.down * speed * deltaTime);
    }

    public bool IsOffScreen() {
        return transform.position.y <= -6f;
    }

    void OnMouseDown() {
        if (isProcessed) return;
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