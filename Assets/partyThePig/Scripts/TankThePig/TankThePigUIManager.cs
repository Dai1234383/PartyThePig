using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private PlayerInput _player1Input; // プレイヤーインプット

    private void Awake()
    {
        Instance = this;
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
    private void Update()
    {
        Debug.Log(_player1Input.currentActionMap);
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
        _toResultButton.gameObject.SetActive(true);
        _gameText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        //テキストを変更
        _gameText.text = (winnerName + " is the WINNER");

        _player1Input.SwitchCurrentActionMap("UI");
    }

}
