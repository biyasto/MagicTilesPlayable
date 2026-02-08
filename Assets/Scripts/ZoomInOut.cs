using System;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
    public float scaleSpeed = 2f;
    public float minScale = 1f;
    public float maxScale = 1.1f;

    private bool isZooming = false;
    private float startTime;
    [SerializeField] private RectTransform rectTransform;
    
    void Update()
    {
        if (!isZooming) return;

        float t = (Mathf.Sin((Time.time - startTime) * scaleSpeed) + 1f) / 2f;
        float scale = Mathf.Lerp(minScale, maxScale, t);

        rectTransform.localScale = Vector3.one * scale;
    }

    public void StartZoom()
    {
        startTime = Time.time;
        isZooming = true;
    }
}