using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankThePigPlayerCtrl : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;     //移動速度
    [SerializeField] private float _rotateSpeed = 200f; //回転速度
    [SerializeField] private int _maxLife = 3;

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;    //画像

    [SerializeField] private GameObject _bulletPrefab;  //弾のPrefab
    [SerializeField] private float bulletSpeed = 10f;   //弾の速度


    private PlayerInput input;

    private int _currentLife;

    private Rigidbody2D _rb;    //RigidBody
    private Vector2 _moveInput; //移動
    private float _rotateInput;  // 回転入力を保存する変数

    private bool _isRotate = false; //回転しているか
    private bool _isMove = false;   //移動しているか

    private bool _isDamageInterval = false;   //無敵時間か

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;  // 自動取得に変更
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }

        _currentLife=_maxLife;
    }

    private void FixedUpdate()
    {
        if (TankThePigGameStateManager.Instance.GameState == TankThePigGameStateManager.GameStateName.GAME)
        {

            // 移動処理（向いている方向に移動）
            Vector2 moveDir = transform.up * _moveInput.y; // 前後方向
            _rb.velocity = moveDir * _moveSpeed;


            // 回転
            if (Mathf.Abs(_rotateInput) > 0.1f)
            {
                _rb.MoveRotation(_rb.rotation + -_rotateInput * _rotateSpeed * Time.fixedDeltaTime);
            }
        }
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (TankThePigGameStateManager.Instance.GameState != TankThePigGameStateManager.GameStateName.GAME) return;

        if (!_isRotate)
        {
            _isMove = true;
            _moveInput = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            _isMove = false;
        }
    }

    /// <summary>
    /// 回転入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    public void OnRotate(InputAction.CallbackContext context)
    {
        if (TankThePigGameStateManager.Instance.GameState != TankThePigGameStateManager.GameStateName.GAME) return;

        if (!_isMove)
        {
            _isRotate = true;
            _rotateInput = context.ReadValue<float>();
        }

        if (context.canceled)
        {
            _isRotate = false;
        }
    }

    /// <summary>
    /// 発射入力
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        // ボタンが「押されたとき」だけ実行
        if (context.started && TankThePigGameStateManager.Instance.GameState == TankThePigGameStateManager.GameStateName.GAME)
        {
            // 弾を生成（プレイヤーの位置 & 向き）
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + transform.up * 0.6f, transform.rotation);

            // BulletController に発射者情報を渡す
            TankThePigBalletCtrl bulletCtrl = bullet.GetComponent<TankThePigBalletCtrl>();
            if (bulletCtrl != null)
            {
                bulletCtrl.shooter = this.gameObject;
            }

            // 弾の色をプレイヤーカラーに設定（SpriteRenderer が付いている前提）
            SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
            if (bulletRenderer != null && PlayerManager.Instance != null)
            {
                bulletRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
            }

            // 弾に Rigidbody2D があるなら、前方に力を加える
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = transform.up * bulletSpeed;
            }
        }
    }

    public void ReceiveBulletHit()
    {
        if (_isDamageInterval == false)
        {
            HitBullet();
        }
    }

    private async void HitBullet()
    {
        _isDamageInterval = true;
        _currentLife--;
        TankThePigUIManager.Instance.UpdateLifeUI(playerIndex,_currentLife);
        if(_currentLife==0)
        {
            string winner;
            if(playerIndex==0)
            {
                winner = "Player2";
            }
            else
            {
                winner = "Player1";
            }

            TankThePigGameStateManager.Instance.GameOver(winner);
        }

        _spriteRenderer.color=Color.clear;
        await UniTask.Delay(300);
        _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        await UniTask.Delay(300);
        _spriteRenderer.color = Color.clear;
        await UniTask.Delay(300);
        _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        await UniTask.Delay(300);
        _spriteRenderer.color = Color.clear;
        await UniTask.Delay(300);
        _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;

        _isDamageInterval =false;
    }
}
