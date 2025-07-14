using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("プレイヤー設定（名前と色）")]
    [SerializeField] private PlayerSetting[] playerSettings = new PlayerSetting[2];

    public PlayerData[] players = new PlayerData[2];

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 設定に基づいてプレイヤーデータを初期化
        for (int i = 0; i < players.Length && i < playerSettings.Length; i++)
        {
            players[i] = new PlayerData(playerSettings[i].playerName, playerSettings[i].playerColor);
        }
    }
}

[System.Serializable]
public class PlayerSetting
{
    public string playerName = "Player";
    public Color playerColor = Color.white;
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public Color playerColor;
    public int score;

    public PlayerData(string name, Color color)
    {
        playerName = name;
        playerColor = color;
        score = 0;
    }
}
