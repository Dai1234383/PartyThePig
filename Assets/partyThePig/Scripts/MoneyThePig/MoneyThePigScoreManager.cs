using UnityEngine;

public class MoneyThePigScoreManager : MonoBehaviour
{
    public static MoneyThePigScoreManager Instance;

    private int _player1Score;
    private int _player2Score;

    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;

    [Header("スコア加算効果音")]
    [SerializeField] private AudioClip scoreSE;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _player1Score = 0;
        _player2Score = 0;
    }

    public void AddScore(string Winner, int Price)
    {
        if (Winner == "Player1")
        {
            _player1Score += Price;
            if (_player1Score < 0)
                _player1Score = 0;
        }
        else
        {
            _player2Score += Price;
            if (_player2Score < 0)
                _player2Score = 0;
        }

        // 🔊 効果音再生
        if (audioSource != null && scoreSE != null)
        {
            audioSource.PlayOneShot(scoreSE);
        }

        // UI更新
        MoneyThePigUIManager.Instance.UpdateScoreUI();
    }
}

