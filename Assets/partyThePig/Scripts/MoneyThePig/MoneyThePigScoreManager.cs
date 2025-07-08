using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyThePigScoreManager : MonoBehaviour
{
    public static MoneyThePigScoreManager Instance;

    private int _player1Score;
    private int _player2Score;

    public int Player1Score=>_player1Score;
    public int Player2Score=>_player2Score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _player1Score=0; 
        _player2Score=0;
    }

    public void AddScore(string Winner,int Price)
    {
        if(Winner=="Player1")
        {
            _player1Score += Price;
            if(_player1Score < 0)
            {
                _player1Score = 0;
            }
        }
        else
        {
            _player2Score += Price;
            if(_player2Score < 0)
            {
                _player2Score = 0;
            }
        }
        MoneyThePigUIManager.Instance.UpdateScoreUI();
    }
}
