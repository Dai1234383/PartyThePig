using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyThePigUIManager : MonoBehaviour
{
    public static MoneyThePigUIManager Instance;

    [SerializeField] private TextMeshProUGUI _gameText;         // カウントダウンなどのテキスト
    [SerializeField] private TextMeshProUGUI _timerText;        // タイマー表示テキスト ←★追加
    [SerializeField] private Button _toResultButton;            // リザルトに行くボタン
    [SerializeField] private Button _restartButton;             // リスタートボタン
    [SerializeField] private TextMeshProUGUI _player1ScoreText;
    [SerializeField] private TextMeshProUGUI _player2ScoreText;

    [SerializeField] private TextMeshProUGUI _P1ScoreText;
    [SerializeField] private TextMeshProUGUI _P2ScoreText;
    [SerializeField] private float _scaleUp = 1.5f; // 大きくなる倍率
    [SerializeField] private float _duration = 0.2f; // アニメーション時間

    private int _player1Score;
    private int _player2Score;

    private void Awake()
    {
        Instance = this;
        _player1Score = PlayerManager.Instance.GetPlayerData(0).score;
        _player2Score = PlayerManager.Instance.GetPlayerData(1).score;
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
        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.GAME)
        {
            float remaining = MoneyThePigGameStateManager.Instance.RemainingTime;
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
        _gameText.gameObject.SetActive(true);
        if (winnerName == "Draw")
        {
            _gameText.text ="Draw";
        }
        else
        {
            _gameText.text = winnerName + " is the WINNER";
        }

        _P1ScoreText.gameObject.SetActive(true);
        _P2ScoreText.gameObject.SetActive(true);
        _P1ScoreText.text = _player1Score.ToString();
        _P2ScoreText.text = _player2Score.ToString();
        if (winnerName == "Player1")
        {
            ChangeNumber(_P1ScoreText, _player1Score + 1);
        }
        else if (winnerName == "Player2")
        {
            ChangeNumber(_P2ScoreText, _player2Score + 1);
        }
    }

    public void UpdateScoreUI()
    {
        _player1ScoreText.text = "P1 " + MoneyThePigScoreManager.Instance.Player1Score.ToString();
        _player2ScoreText.text = "P2 " + MoneyThePigScoreManager.Instance.Player2Score.ToString();
    }

    /// <summary>
    /// 数字をアニメーション付きで変更
    /// </summary>
    public void ChangeNumber(TextMeshProUGUI WinnerText, int WinnerScore)
    {
        // DOTweenのシーケンスを作成
        Sequence seq = DOTween.Sequence();

        seq.Append(WinnerText.transform.DOScale(_scaleUp, _duration * 10)) // ① 大きくなる
           .AppendCallback(() => WinnerText.text = WinnerScore.ToString()) // ② 数字を変更
           .Append(WinnerText.transform.DOScale(1f, _duration)) // ③ 元に戻る
           .SetEase(Ease.OutBack) // アニメーションを柔らかくする
           .OnComplete(() => { ActiveButton(); });
    }

    private void ActiveButton()
    {
        _toResultButton.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);

    }
}
