using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootThePigScoreManager : MonoBehaviour
{
    public static ShootThePigScoreManager Instance;

    private int _player1Score;
    private int _player2Score;

    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _player1Score = 0;
        _player2Score = 0;
    }

    public void AddScore(string Hitter, int TargetScore ,bool isRepaint)
    {
        if (Hitter == "Player1")
        {
            _player1Score += TargetScore;
            if(isRepaint)
            {
                _player2Score -= TargetScore;
            }
            if (_player1Score < 0)
            {
                _player1Score = 0;
            }
        }
        else
        {
            _player2Score += TargetScore;
            if (isRepaint)
            {
                _player1Score -= TargetScore;
            }

            if (_player2Score < 0)
            {
                _player2Score = 0;
            }
        }
        ShootThePigUIManager.Instance.UpdateScoreUI();
    }
}
