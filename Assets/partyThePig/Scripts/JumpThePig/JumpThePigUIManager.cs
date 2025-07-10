using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JumpThePigUIManager : MonoBehaviour
{
    public static JumpThePigUIManager Instance;

    [SerializeField] private TextMeshProUGUI _gameText;     //カウントダウンなどのテキスト
    [SerializeField] private Button _toResultButton;        //リザルトに行くボタン
    [SerializeField] private Button _restartButton;         //リスタートボタン

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        _gameText.text = ("3");
        await UniTask.Delay(1000);
        _gameText.text = ("2");
        await UniTask.Delay(1000);
        _gameText.text = ("1");
        await UniTask.Delay(1000);
        _gameText.text = ("GameStart!!");
        await UniTask.Delay(1000);
        _gameText.gameObject.SetActive(false);
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
    }
}
