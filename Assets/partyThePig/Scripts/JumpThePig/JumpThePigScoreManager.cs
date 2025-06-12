using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpThePigScoreManager : MonoBehaviour
{
    public static JumpThePigScoreManager Instance;

    [SerializeField] private Transform _playerTransform;    // プレイヤーの座標

    private int _maxScore;        //最大スコア
    private int _currentScore;    //現在のスコア

    public int MaxScore => _maxScore;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _maxScore = 0;
    }
    private void Update()
    {
        AcquireScore();
    }

    /// <summary>
    /// スコアの計測
    /// </summary>
    private void AcquireScore()
    {
        // 現在のスコアを取得
        _currentScore = Mathf.FloorToInt(_playerTransform.position.y + 1);

        // 最高スコアよりも現在のスコアが高ければ更新
        if (_maxScore < _currentScore)
        {
            _maxScore = _currentScore;
        }
    }
}
