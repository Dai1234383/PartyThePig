using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankThePigBalletCtrl : MonoBehaviour
{
    [SerializeField] private int _groundLayerNum;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _maxBounceCount = 3;

    public GameObject shooter; // 弾を撃ったプレイヤー

    private Vector2 _direction; // 現在の移動方向
    private int _currentBounceCount;

    private void Start()
    {
        // 初期方向を設定（前方）
        _direction = transform.up;
    }

    private void Update()
    {
        // 手動で移動（物理を使わない）
        transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 自分自身には当たらないようにする
        if (collision.gameObject == shooter)
        {
            if (_currentBounceCount != 0)
            {
                Destroy(gameObject);
            }
            return;
        }
        // プレイヤーに当たったら処理
        TankThePigPlayerCtrl hitPlayer = collision.gameObject.GetComponent<TankThePigPlayerCtrl>();
        if (hitPlayer != null)
        {
            hitPlayer.ReceiveBulletHit();
            Destroy(gameObject);
            return;
        }

        // 壁に当たったら反射
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _currentBounceCount++;

            // 反射方向を計算
            Vector2 normal = collision.contacts[0].normal;
            _direction = Vector2.Reflect(_direction, normal).normalized;

            if (_currentBounceCount >= _maxBounceCount)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // 他のものに当たったら削除
            Destroy(gameObject);
        }
    }
}
