using UnityEngine;

public class MoneyThePigBomArea : MonoBehaviour
{
    [SerializeField] private int _bomDamage;

    private MoneyThePigPlayerCtrl _playerCtrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerCtrl = collision.GetComponent<MoneyThePigPlayerCtrl>();

            if (_playerCtrl.PlayerIndex == 0)
            {
                MoneyThePigScoreManager.Instance.AddScore("Player1", -_bomDamage);
            }
            else if (_playerCtrl.PlayerIndex == 1)
            {
                MoneyThePigScoreManager.Instance.AddScore("Player2", -_bomDamage);
            }
        }
    }
}
