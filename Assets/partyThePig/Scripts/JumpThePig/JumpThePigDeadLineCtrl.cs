using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class JumpThePigDeadLineCtrl : MonoBehaviour
{

    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;

    private string _winnerName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == _playerTag && JumpThePigGameStateManager.Instance.GameState == JumpThePigGameStateManager.GameStateName.GAME)
        {
            if (collision.gameObject == _player1)
            {
                _winnerName = "PLAYER2";
            }
            else if (collision.gameObject == _player2)
            {
                _winnerName = "PLAYER1";
            }
            JumpThePigGameStateManager.Instance.GameOver(_winnerName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (((1 << other.gameObject.layer) & _groundLayer) != 0)
        {
            Debug.Log("aaa");
            Destroy(other.gameObject);
        }
    }
}
