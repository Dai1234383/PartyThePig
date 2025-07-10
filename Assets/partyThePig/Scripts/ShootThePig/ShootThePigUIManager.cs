using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShootThePigUIManager : MonoBehaviour
{
    public static ShootThePigUIManager Instance;

    [SerializeField] private TextMeshProUGUI _gameText;         // カウントダウンなどのテキスト
    [SerializeField] private TextMeshProUGUI _timerText;        // タイマー表示テキスト
    [SerializeField] private Button _toResultButton;            // リザルトに行くボタン
    [SerializeField] private Button _restartButton;             // リスタートボタン
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        UpdateScoreUI();

        _gameText.text = "3";
        await UniTask.Delay(1000);
        _gameText.text = "2";
        await UniTask.Delay(1000);
        _gameText.text = "1";
        await UniTask.Delay(1000);
        _gameText.text = "GameStart!!";
        await UniTask.Delay(1000);
        _gameText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // ゲーム中のみタイマーを表示
        if (ShootThePigGameStateManager.Instance.GameState == ShootThePigGameStateManager.GameStateName.GAME)
        {
            float remaining = ShootThePigGameStateManager.Instance.RemainingTime;
            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            _timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    /// <summary>
    /// ゲームオーバー時にUIを変更
    /// </summary>
    public void GameOverUI(string winnerName)
    {
        _toResultButton.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _gameText.gameObject.SetActive(true);
        if (winnerName == "Draw")
        {
            _gameText.text = "Draw";
        }
        else
        {
            _gameText.text = winnerName + " is the WINNER";
        }
    }

    public void UpdateScoreUI()
    {
        _player1ScoreText.text = "P1 " + ShootThePigScoreManager.Instance.Player1Score.ToString();
        _player2ScoreText.text = "P2 " + ShootThePigScoreManager.Instance.Player2Score.ToString();
    }
}
