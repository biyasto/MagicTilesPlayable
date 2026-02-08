using System;
using UnityEngine;

public class SpriteAlphaFade : MonoBehaviour
{
    public Action onFadeComplete;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float fadeDuration = 1f;   // fade time
    [SerializeField] private float completeDelay = 0.5f; // wait AFTER fade

    private float elapsed;
    private float delayElapsed;

    private float startAlpha = 0f;
    private float targetAlpha = 230f / 255f;

    private bool isFading = false;
    private bool isWaiting = false;

    private void OnEnable()
    {
        StartFade();
    }

    private void Update()
    {
        if (isFading)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            Color c = sprite.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            sprite.color = c;

            if (t >= 1f)
            {
                isFading = false;
                isWaiting = true;
                delayElapsed = 0f;
            }
            return;
        }

        if (isWaiting)
        {
            delayElapsed += Time.deltaTime;
            if (delayElapsed >= completeDelay)
            {
                isWaiting = false;
                onFadeComplete?.Invoke();
            }
        }
    }

    public void StartFade()
    {
        elapsed = 0f;
        delayElapsed = 0f;
        isFading = true;
        isWaiting = false;

        Color c = sprite.color;
        c.a = startAlpha;
        sprite.color = c;
    }
}