using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShootThePigGameStateManager : MonoBehaviour
{
    public static ShootThePigGameStateManager Instance;

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

    [Header("制限時間（秒）")]
    [SerializeField] private float _timeLimit = 60f;

    private float _remainingTime;
    public float RemainingTime => _remainingTime;

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
        TimerStart().Forget(); // タイマー開始
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
    public void GameOver(string winnerName = "")
    {
        ShootThePigUIManager.Instance.GameOverUI(winnerName);
        if (winnerName == "Player1")
        {
            PlayerManager.Instance.GetPlayerData(0).score += 1;
        }
        else if (winnerName == "Player2")
        {
            PlayerManager.Instance.GetPlayerData(1).score += 1;
        }
        _gameState = GameStateName.OVER;
    }

    /// <summary>
    /// 制限時間カウントダウン
    /// </summary>
    private async UniTaskVoid TimerStart()
    {
        _remainingTime = _timeLimit;

        while (_remainingTime > 0 && _gameState == GameStateName.GAME)
        {
            await UniTask.DelayFrame(1); // 毎フレーム待機
            _remainingTime -= Time.deltaTime;
        }

        if (_gameState == GameStateName.GAME)
        {
            if (ShootThePigScoreManager.Instance.Player1Score > ShootThePigScoreManager.Instance.Player2Score)
            {
                GameOver("Player1");
            }
            else if (ShootThePigScoreManager.Instance.Player1Score < ShootThePigScoreManager.Instance.Player2Score)
            {
                GameOver("Player2");
            }
            else
            {
                GameOver("Draw");
            }

        }
    }
}
