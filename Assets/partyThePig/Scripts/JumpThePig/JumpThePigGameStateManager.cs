using Cysharp.Threading.Tasks;
using UnityEngine;

public class JumpThePigGameStateManager : MonoBehaviour
{
    public static JumpThePigGameStateManager Instance;

    /// <summary>
    /// インゲームの状態管理
    /// </summary>
    public enum GameStateName
    {
        START,
        GAME,
        CLEAR,
        OVER,
    }


    private GameStateName _gameState;
    public GameStateName GameState => _gameState;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        _gameState = GameStateName.START;

        // 3秒待機
        await UniTask.Delay(3000);
        GameStart();
    }

    /// <summary>
    /// ゲームスタート処理
    /// </summary>
    public void GameStart()
    {
        _gameState = GameStateName.GAME;
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear()
    {
        _gameState = GameStateName.CLEAR;
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver(string winnerName)
    {
        JumpThePigUIManager.Instance.GameOverUI(winnerName);
        _gameState = GameStateName.OVER;
    }
}
