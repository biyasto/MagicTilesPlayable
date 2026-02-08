using UnityEngine;

public class EndGameUI : MonoBehaviour
{
  
    [SerializeField] private ZoomInOut zoom;
    [SerializeField] private MovePingPong pingPong;
    [SerializeField] private GameObject endgameBg;

    [ContextMenu("Show UI")]
    public void ShowUI()
    {
        endgameBg.gameObject.SetActive(true);
        gameObject.SetActive(true);
        zoom.enabled = true;
        pingPong.enabled = true;
        zoom.StartZoom();
        pingPong.StartMove();
        Luna.Unity.LifeCycle.GameEnded();
    }

    
    private bool clicked = false;

    public void OnCTAClick()
    {
        if (clicked) return;
        clicked = true;
        Luna.Unity.Playable.InstallFullGame();
    }
}