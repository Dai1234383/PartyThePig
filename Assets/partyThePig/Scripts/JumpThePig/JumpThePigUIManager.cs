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
    [SerializeField] private TextMeshProUGUI _scoreText;    //スコア表示のテキスト

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
    private void Update()
    {
        UpdateScore();
    }

    /// <summary>
    /// スコアの更新
    /// </summary>
    private void UpdateScore()
    {
        int score = JumpThePigScoreManager.Instance.MaxScore;
        string scoreText = score.ToString();
        _scoreText.text = ("score:" + scoreText + " m");
    }


    /// <summary>
    /// ゲームオーバー時にUIを変更
    /// </summary>
    public void GameOverUI()
    {
        //セットアクティブを切り替え
        _toResultButton.gameObject.SetActive(true);
        _gameText.gameObject.SetActive(true);
        //テキストを変更
        _gameText.text = ("GameOver!!");
    }

}
