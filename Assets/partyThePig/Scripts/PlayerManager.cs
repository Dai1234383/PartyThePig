using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

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

        // プレイヤー情報の初期化（仮）
        players[0] = new PlayerData("Player1", Color.red);
        players[1] = new PlayerData("Player2", Color.blue);
    }
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
