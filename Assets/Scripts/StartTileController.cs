
    using System;
    using UnityEngine;

    public class StartTileController : MonoBehaviour
    {
        public Action onTileClicked;
        [SerializeField] private BoxCollider2D tileCollider;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClick(Input.mousePosition);
            }
        }

        void CheckClick(Vector2 screenPos)
        {

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null && hit.collider == tileCollider)
            {
                onTileClicked?.Invoke();
            }
        }
    }
