using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class EscapeThePigUIManager : MonoBehaviour
{
    public static EscapeThePigUIManager Instance;

    [Header("ゲーム進行UI")]
    [SerializeField] private TextMeshProUGUI _gameText;
    [SerializeField] private Button _toResultButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _P1ScoreText;
    [SerializeField] private TextMeshProUGUI _P2ScoreText;
    [SerializeField] private float _scaleUp = 1.5f; // 大きくなる倍率
    [SerializeField] private float _duration = 0.2f; // アニメーション時間

    [Header("プレイヤーごとのアイテムアイコンUI")]
    [SerializeField] private List<Image> _playerItemIcons = new(); // Image型に変更！

    [SerializeField] private Sprite _defaultIcon; // アイテムなし時のデフォルト（透明画像など）

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
        _gameText.text = "3";
        await UniTask.Delay(1000);
        _gameText.text = "2";
        await UniTask.Delay(1000);
        _gameText.text = "1";
        await UniTask.Delay(1000);
        _gameText.text = "GameStart!!";
        await UniTask.Delay(1000);
        _gameText.gameObject.SetActive(false);

        SetupItemUIListeners();
    }

    private void SetupItemUIListeners()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length && i < _playerItemIcons.Count; i++)
        {
            var itemSystem = players[i].GetComponent<EscapeThePigPlayerItemSystem>();
            int index = i;

            if (itemSystem != null)
            {
                UpdateItemIcon(index, itemSystem.GetHeldItem());

                itemSystem.OnItemChanged.AddListener((item) =>
                {
                    UpdateItemIcon(index, item);
                });
            }
        }
    }

    private void UpdateItemIcon(int index, GameObject item)
    {
        if (index < 0 || index >= _playerItemIcons.Count) return;

        Image iconImage = _playerItemIcons[index];

        if (item != null)
        {
            var icon = item.GetComponent<EscapeThePigItemIcon>();
            if (icon != null && icon.itemIcon != null)
            {
                iconImage.sprite = icon.itemIcon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = _defaultIcon;
                iconImage.enabled = true;
            }
        }
        else
        {
            iconImage.sprite = _defaultIcon;
            iconImage.enabled = true;
        }
    }

    public void GameOverUI(string winnerName)
    {
        _gameText.gameObject.SetActive(true);
        _gameText.text = winnerName + " is the WINNER";
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

        seq.Append(WinnerText.transform.DOScale(_scaleUp, _duration * 3)) // ① 大きくなる
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
