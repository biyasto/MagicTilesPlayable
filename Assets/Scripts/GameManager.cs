using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public struct TileEvent
    {
        public int id;
        public float time;
        public float duration;
        public int lane;
    }

    [Header("Prefabs")]
    public GameObject shortTilePrefab;
    public GameObject longTilePrefab;

    [Header("Movement Settings")]
    public float fallSpeed = 4f;
    public float spawnY = 3f;
    public float bottomY = -3f;
    private readonly float[] laneXPositions = { -1.05f, -0.35f, 0.35f, 1.05f };

    [Header("State")]
    private float timer = 0f;
    private int currentIndex = 0;
    private bool gameActive = false;

    private Queue<GameObject> shortPool = new Queue<GameObject>();
    private Queue<GameObject> longPool = new Queue<GameObject>();

    [SerializeField] CameraShake cameraShake;
    [SerializeField] StartGameUI startGameUI;
    [SerializeField] EndGameUI endGameUI;
    [SerializeField] Transform tilesParent;
    [SerializeField] SpriteAlphaFade fade;
    [SerializeField] public Sprite headDeadSprite;
    [SerializeField] public Sprite headSprite;


    private List<GameObject> activeTiles = new List<GameObject>();

    void Awake()
    {
        // 🔥 ultra-light singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameActive = false;
        startGameUI.onGameStart = () =>
        {
            gameActive = true;
            timer = 0f;
            currentIndex = 0;
        };
        Luna.Unity.LifeCycle.GameLoaded();

    }

    void Update()
    {
        if (!gameActive) return;
        timer += Time.deltaTime;

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            var controller = activeTiles[i].GetComponent<TileController>();
            controller.MoveDown(fallSpeed, Time.deltaTime);

            if (controller.transform.position.y<bottomY && controller.isTouched==false)
            {
             //   EndGame();
            }

            if (controller.IsOffScreen())
            {
                bool isLong = activeTiles[i].name.Contains("Long");
                ReturnToPool(activeTiles[i], isLong);
                activeTiles.RemoveAt(i);
            }
        }

        while (currentIndex < levelData.Length && timer >= levelData[currentIndex].time)
        {
            Spawn(levelData[currentIndex]);
            currentIndex++;
        }

        if (timer > 29f)
        {
            EndGame();
        }
    }

    void Spawn(TileEvent e)
    {
        bool isLong = e.duration > 0.5f;
        GameObject obj = isLong
            ? Instantiate(longTilePrefab)
            : (shortPool.Count > 0 ? shortPool.Dequeue() : Instantiate(shortTilePrefab));

        obj.transform.SetParent(tilesParent);
        obj.transform.position = new Vector3(laneXPositions[e.lane], spawnY, 0);
        obj.SetActive(true);

        var controller = obj.GetComponent<TileController>();
        controller.Activate(() => ReturnToPool(obj, isLong));

        activeTiles.Add(obj);
    }

    void ReturnToPool(GameObject obj, bool isLong)
    {
        obj.SetActive(false);
        var controller = obj.GetComponent<TileController>();
        controller.Reset();

        if (isLong) longPool.Enqueue(obj);
        else shortPool.Enqueue(obj);
    }

    void EndGame()
    {
        AudioManager.Instance.StopMusic();
        gameActive = false;
        fade.enabled = true;
        fade.onFadeComplete = ShowEndGameUI;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.overSound);
    }

    void ShowEndGameUI()
    {
        tilesParent.gameObject.SetActive(false);
        endGameUI.ShowUI();
    }

    public void ShakeCamera()
    {
        cameraShake.Shake();
    }

    public bool IsGameActive() => gameActive;
    
    #region level data 
    private readonly TileEvent[] levelData = { new TileEvent{id=0, time=0.000000f, duration=0.285715f, lane=3}, new TileEvent{id=1, time=0.190476f, duration=0.285715f, lane=2}, new TileEvent{id=2, time=0.380953f, duration=0.285715f, lane=1}, new TileEvent{id=3, time=0.571429f, duration=0.285715f, lane=0}, new TileEvent{id=4, time=0.952382f, duration=0.285715f, lane=3}, new TileEvent{id=5, time=1.142858f, duration=0.285715f, lane=0}, new TileEvent{id=6, time=2.285716f, duration=0.285715f, lane=0}, new TileEvent{id=7, time=2.476192f, duration=0.285715f, lane=1}, new TileEvent{id=8, time=2.666669f, duration=0.285715f, lane=2}, new TileEvent{id=9, time=2.857145f, duration=0.285715f, lane=3}, new TileEvent{id=10, time=3.238098f, duration=0.285715f, lane=0}, new TileEvent{id=11, time=3.428574f, duration=0.285715f, lane=3}, new TileEvent{id=12, time=4.571432f, duration=0.285715f, lane=0}, new TileEvent{id=13, time=4.952385f, duration=0.285715f, lane=2}, new TileEvent{id=14, time=5.142861f, duration=0.285715f, lane=0}, new TileEvent{id=15, time=5.476195f, duration=0.285715f, lane=3}, new TileEvent{id=16, time=5.714290f, duration=0.285715f, lane=1}, new TileEvent{id=17, time=6.095243f, duration=0.285715f, lane=2}, new TileEvent{id=18, time=6.285719f, duration=0.285715f, lane=0}, new TileEvent{id=19, time=6.666672f, duration=0.285715f, lane=3}, new TileEvent{id=20, time=6.857148f, duration=0.285715f, lane=1}, new TileEvent{id=21, time=7.428577f, duration=0.285715f, lane=3}, new TileEvent{id=22, time=7.428577f, duration=0.285715f, lane=1}, new TileEvent{id=23, time=8.000006f, duration=1.142858f, lane=2}, new TileEvent{id=24, time=8.000006f, duration=1.142858f, lane=0}, new TileEvent{id=25, time=9.142864f, duration=0.285715f, lane=3}, new TileEvent{id=26, time=9.333340f, duration=0.285715f, lane=2}, new TileEvent{id=27, time=9.523817f, duration=0.285715f, lane=1}, new TileEvent{id=28, time=9.714293f, duration=0.285715f, lane=0}, new TileEvent{id=29, time=10.095246f, duration=0.285715f, lane=3}, new TileEvent{id=30, time=10.285722f, duration=0.285715f, lane=0}, new TileEvent{id=31, time=10.857151f, duration=0.285715f, lane=3}, new TileEvent{id=32, time=10.857151f, duration=0.285715f, lane=0}, new TileEvent{id=33, time=11.428580f, duration=0.285715f, lane=0}, new TileEvent{id=34, time=11.619056f, duration=0.285715f, lane=1}, new TileEvent{id=35, time=11.809533f, duration=0.285715f, lane=2}, new TileEvent{id=36, time=12.000009f, duration=0.285715f, lane=3}, new TileEvent{id=37, time=12.380962f, duration=0.285715f, lane=0}, new TileEvent{id=38, time=12.571438f, duration=0.285715f, lane=3}, new TileEvent{id=39, time=13.142867f, duration=0.285715f, lane=3}, new TileEvent{id=40, time=13.142867f, duration=0.285715f, lane=0}, new TileEvent{id=41, time=13.714296f, duration=0.285715f, lane=3}, new TileEvent{id=42, time=14.095249f, duration=0.285715f, lane=2}, new TileEvent{id=43, time=14.095249f, duration=0.285715f, lane=0}, new TileEvent{id=44, time=14.285725f, duration=0.285715f, lane=3}, new TileEvent{id=45, time=14.666678f, duration=0.285715f, lane=2}, new TileEvent{id=46, time=14.666678f, duration=0.285715f, lane=0}, new TileEvent{id=47, time=14.857154f, duration=0.285715f, lane=1}, new TileEvent{id=48, time=15.238107f, duration=0.285715f, lane=2}, new TileEvent{id=49, time=15.428583f, duration=0.285715f, lane=1}, new TileEvent{id=50, time=15.809536f, duration=0.285715f, lane=3}, new TileEvent{id=51, time=16.000012f, duration=0.285715f, lane=0}, new TileEvent{id=52, time=16.380965f, duration=0.285715f, lane=2}, new TileEvent{id=53, time=16.571441f, duration=0.285715f, lane=1}, new TileEvent{id=54, time=16.952394f, duration=0.285715f, lane=3}, new TileEvent{id=55, time=17.142870f, duration=0.285715f, lane=0}, new TileEvent{id=56, time=18.285728f, duration=0.285715f, lane=0}, new TileEvent{id=57, time=18.476204f, duration=0.285715f, lane=1}, new TileEvent{id=58, time=18.666681f, duration=0.285715f, lane=2}, new TileEvent{id=59, time=18.857157f, duration=0.285715f, lane=3}, new TileEvent{id=60, time=19.238110f, duration=0.285715f, lane=0}, new TileEvent{id=61, time=19.428586f, duration=0.285715f, lane=3}, new TileEvent{id=62, time=20.000015f, duration=0.285715f, lane=3}, new TileEvent{id=63, time=20.000015f, duration=0.285715f, lane=0}, new TileEvent{id=64, time=20.571444f, duration=0.285715f, lane=3}, new TileEvent{id=65, time=20.761920f, duration=0.285715f, lane=2}, new TileEvent{id=66, time=20.952397f, duration=0.285715f, lane=1}, new TileEvent{id=67, time=21.142873f, duration=0.285715f, lane=0}, new TileEvent{id=68, time=21.523826f, duration=0.285715f, lane=3}, new TileEvent{id=69, time=21.714302f, duration=0.285715f, lane=0}, new TileEvent{id=70, time=22.285731f, duration=0.285715f, lane=3}, new TileEvent{id=71, time=22.285731f, duration=0.285715f, lane=0}, new TileEvent{id=72, time=22.857160f, duration=0.285715f, lane=1}, new TileEvent{id=73, time=23.238113f, duration=0.285715f, lane=2}, new TileEvent{id=74, time=23.238113f, duration=0.285715f, lane=0}, new TileEvent{id=75, time=23.428589f, duration=0.285715f, lane=3}, new TileEvent{id=76, time=23.809542f, duration=0.285715f, lane=3}, new TileEvent{id=77, time=23.809542f, duration=0.285715f, lane=1}, new TileEvent{id=78, time=24.000018f, duration=0.285715f, lane=0}, new TileEvent{id=79, time=24.380971f, duration=0.285715f, lane=2}, new TileEvent{id=80, time=24.380971f, duration=0.285715f, lane=0}, new TileEvent{id=81, time=24.571447f, duration=0.285715f, lane=3}, new TileEvent{id=82, time=24.952400f, duration=0.285715f, lane=2}, new TileEvent{id=83, time=24.952400f, duration=0.285715f, lane=0}, new TileEvent{id=84, time=25.142876f, duration=0.285715f, lane=3}, new TileEvent{id=85, time=25.142876f, duration=0.285715f, lane=1}, new TileEvent{id=86, time=25.714305f, duration=0.285715f, lane=2}, new TileEvent{id=87, time=25.714305f, duration=0.285715f, lane=0}, new TileEvent{id=88, time=26.285734f, duration=0.285715f, lane=3}, new TileEvent{id=89, time=26.285734f, duration=0.285715f, lane=1} }; 
    #endregion
}
