using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LunaPlayableArea : MonoBehaviour
{
    public float referenceWidth = 1080f;
    public float referenceHeight = 1920f;
    public float unitsPerPixel = 0.01f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = referenceWidth / referenceHeight;

        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = (referenceHeight * 0.5f) * unitsPerPixel;
        }
        else
        {
            float scale = targetRatio / screenRatio;
            cam.orthographicSize = (referenceHeight * 0.5f) * unitsPerPixel * scale;
        }
    }
}