using UnityEngine;

public class MoneyThePigCoinCtrl : MonoBehaviour
{
    [SerializeField] private int _coinPrice;

    private MoneyThePigPlayerCtrl _playerCtrl;

    private void FixedUpdate()
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerCtrl=collision.GetComponent<MoneyThePigPlayerCtrl>();

            if (_playerCtrl.PlayerIndex==0)
            {
                MoneyThePigScoreManager.Instance.AddScore("Player1", _coinPrice);
            }
            else if (_playerCtrl.PlayerIndex == 1)
            {
                MoneyThePigScoreManager.Instance.AddScore("Player2", _coinPrice);
            }
            Destroy(gameObject);
        }

    }
}
