using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _1PScore;
    [SerializeField] private TextMeshProUGUI _2PScore;


    private void Start()
    {
        UpdateScore();
    }

    public void ResetScore()
    {
        PlayerManager.Instance.GetPlayerData(0).score = 0;
        PlayerManager.Instance.GetPlayerData(1).score = 0;
        UpdateScore();
    }

    public void UpdateScore()
    {
        _1PScore.text = PlayerManager.Instance.GetPlayerData(0).score.ToString();
        _2PScore.text = PlayerManager.Instance.GetPlayerData(1).score.ToString();
    }
}
