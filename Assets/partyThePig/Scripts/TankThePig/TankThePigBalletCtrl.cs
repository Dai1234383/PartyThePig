using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class TankThePigBalletCtrl : MonoBehaviour
{
    [SerializeField] private int _groundLayerNum;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _maxBounceCount = 3;

    public GameObject shooter; // 弾を撃ったプレイヤー

    private Rigidbody2D _rb;
    private int _currentBounceCount;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;             // 弾なので重力無効
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // すり抜け防止
        _rb.freezeRotation = true;        // 回転は固定

        // 初期速度を設定
        _rb.velocity = -transform.right * _speed;
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

        // 壁に当たった場合
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _currentBounceCount++;
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
