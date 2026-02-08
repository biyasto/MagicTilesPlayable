using System;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    public Action onGameStart;
    [SerializeField] private MovePingPong pingPong;
    [SerializeField] private StartTileController startTileController;

    private void Start()
    {
        ShowUI();
    }

    [ContextMenu("Show UI")]
    void ShowUI()
    {
        gameObject.SetActive(true);
        pingPong.enabled = true;
        pingPong.StartMove();
        startTileController.onTileClicked = GameStart;
    }
    
    private void GameStart()
    {
        Luna.Unity.LifeCycle.GameStarted();

        Destroy(startTileController.gameObject);
        Destroy(pingPong.gameObject);
        onGameStart?.Invoke();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.tapSound);
        gameObject.SetActive(false);
    }
}