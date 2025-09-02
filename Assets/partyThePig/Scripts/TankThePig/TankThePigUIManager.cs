using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankThePigUIManager : MonoBehaviour
{
    public static TankThePigUIManager Instance;

    [SerializeField] private TextMeshProUGUI _gameText;     //カウントダウンなどのテキスト
    [SerializeField] private Button _toResultButton;        //リザルトに行くボタン
    [SerializeField] private Button _restartButton;         //リスタートボタン

    [SerializeField] private TextMeshProUGUI _player1LifeText;
    [SerializeField] private TextMeshProUGUI _player2LifeText;
    [SerializeField] private TextMeshProUGUI _P1ScoreText;
    [SerializeField] private TextMeshProUGUI _P2ScoreText;
    [SerializeField] private float _scaleUp = 1.5f; // 大きくなる倍率
    [SerializeField] private float _duration = 0.2f; // アニメーション時間

    [SerializeField] private PlayerInput _player1Input; // プレイヤーインプット

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
        _player1LifeText.gameObject.SetActive(false);
        _player2LifeText.gameObject.SetActive(false);

        _gameText.text = ("3");
        await UniTask.Delay(1000);
        _gameText.text = ("2");
        await UniTask.Delay(1000);
        _gameText.text = ("1");
        await UniTask.Delay(1000);
        _gameText.text = ("GameStart!!");
        GameStartUI();
        await UniTask.Delay(1000);
        _gameText.gameObject.SetActive(false);
    }
    

    /// <summary>
    /// 残りライフ数の更新処理
    /// </summary>
    /// <param name="PlayerNum"></param>
    /// <param name="PlayerLife"></param>
    public void UpdateLifeUI(int PlayerNum,int PlayerLife)
    {
        if(PlayerNum==0)
        {
            _player1LifeText.text = ("Life " + PlayerLife);
        }
        else
        {
            _player2LifeText.text = ("Life " + PlayerLife);
        }
    }

    public void GameStartUI()
    {
        _player1LifeText.gameObject.SetActive(true);
        _player2LifeText.gameObject.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバー時にUIを変更
    /// </summary>
    public void GameOverUI(string winnerName)
    {
        //セットアクティブを切り替え
        _gameText.gameObject.SetActive(true);
        //テキストを変更
        _gameText.text = (winnerName + " is the WINNER");
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
