using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankThePigBalletCtrl : MonoBehaviour
{
    [SerializeField] private int _groundLayerNum;
    [SerializeField] private string _playerTag = "Player";
    public GameObject shooter; // 弾を撃ったプレイヤー

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 自分自身には当たらないようにする
        if (collision.gameObject == shooter) return;

        // 相手プレイヤーのスクリプトを取得
        TankThePigPlayerCtrl hitPlayer = collision.gameObject.GetComponent<TankThePigPlayerCtrl>();
        if (hitPlayer != null)
        {
            hitPlayer.ReceiveBulletHit();
        }

        // 弾を削除
        Destroy(gameObject);
    }
}
