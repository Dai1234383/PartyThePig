using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー情報（名前、色、スコア、コントローラーのデバイス）や初期位置を管理するシングルトン
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("プレイヤー設定（名前と色）")]
    [SerializeField] private PlayerSetting[] playerSettings = new PlayerSetting[2];

    // プレイヤーデータ本体（スコアや色など）
    public PlayerData[] players = new PlayerData[2];

    // プレイヤーごとのInputDevice（コントローラー）を記録
    private InputDevice[] assignedDevices = new InputDevice[2];

    // 各ミニゲームごとに変更できるスタート位置
    private Vector2[] currentStartPositions = new Vector2[2]
    {
        new Vector2(-4, 0), // プレイヤー0のデフォルト位置（左）
        new Vector2(4, 0)   // プレイヤー1のデフォルト位置（右）
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // プレイヤーデータ初期化
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new PlayerData(playerSettings[i].playerName, playerSettings[i].playerColor);
        }
    }

    /// <summary>
    /// 最初に接続されたデバイスをプレイヤーに紐付けて記憶
    /// </summary>
    public void AssignDevice(int playerIndex, InputDevice device)
    {
        if (playerIndex < 0 || playerIndex >= assignedDevices.Length) return;

        if (assignedDevices[playerIndex] == null)
        {
            assignedDevices[playerIndex] = device;
        }
    }

    /// <summary>
    /// 保存しておいたデバイスを返す（再ペアリング用）
    /// </summary>
    public InputDevice GetDevice(int playerIndex)
    {
        return (playerIndex >= 0 && playerIndex < assignedDevices.Length) ? assignedDevices[playerIndex] : null;
    }

    /// <summary>
    /// ミニゲーム側からスタート位置を設定できる
    /// </summary>
    public void SetStartPositions(Vector2 leftPlayer, Vector2 rightPlayer)
    {
        currentStartPositions[0] = leftPlayer;
        currentStartPositions[1] = rightPlayer;
    }

    /// <summary>
    /// プレイヤーごとの現在のスタート位置を取得
    /// </summary>
    public Vector2 GetStartPosition(int playerIndex)
    {
        return (playerIndex >= 0 && playerIndex < currentStartPositions.Length) ?
            currentStartPositions[playerIndex] : Vector2.zero;
    }

    /// <summary>
    /// プレイヤーデータを取得（色や名前など）
    /// </summary>
    public PlayerData GetPlayerData(int index)
    {
        return (index >= 0 && index < players.Length) ? players[index] : null;
    }
}

/// <summary>
/// プレイヤー設定用（インスペクター用）
/// </summary>
[System.Serializable]
public class PlayerSetting
{
    public string playerName = "Player";
    public Color playerColor = Color.white;
}

/// <summary>
/// 実行時に管理するプレイヤーデータ
/// </summary>
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
