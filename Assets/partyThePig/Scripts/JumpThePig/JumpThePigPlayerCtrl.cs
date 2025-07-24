using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;

/// <summary>
/// プレイヤーの操作スクリプト（InputSystem + Rigidbody2Dで移動とジャンプ）
/// </summary>
public class JumpThePigPlayerCtrl : MonoBehaviour
{
    [Header("プレイヤー設定")]
    [SerializeField] private int playerIndex; // 0 = 左, 1 = 右（シーンにあらかじめ設定）
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _collision;
    [SerializeField] private InputActionAsset _action;

    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private JumpThePigCollision _jumpCollision;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _jumpCollision = _collision.GetComponent<JumpThePigCollision>();
    }

    private void Start()
    {
        // スタート位置をプレイヤーインデックスで設定
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

        // 色をプレイヤー設定から適用
        var data = PlayerManager.Instance.GetPlayerData(playerIndex);
        if (data != null)
        {
            _spriteRenderer.color = data.playerColor;
        }


        // 初回の接続デバイスを保存（未保存時のみ）
        var currentDevice = _playerInput.devices.Count > 0 ? _playerInput.devices[0] : null;
        if (currentDevice != null)
        {
            PlayerManager.Instance.AssignDevice(playerIndex, currentDevice);
        }

        // 保存済みのデバイスを再ペアリング（シーン再読み込み時など）
        var savedDevice = PlayerManager.Instance.GetDevice(playerIndex);
        if (savedDevice != null)
        {
            _playerInput.user.UnpairDevices(); // デバイスだけ解除（ユーザーは残す）
            InputUser.PerformPairingWithDevice(savedDevice, _playerInput.user); // 再ペアリング
        }
    }

    private void FixedUpdate()
    {
        if (JumpThePigGameStateManager.Instance.GameState == JumpThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME)
            return;

        var velocity = _rb.velocity;
        velocity.x = _moveInput.x * _moveSpeed;
        _rb.velocity = velocity;
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events 経由）
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME) return;
        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ジャンプ入力（Invoke Unity Events 経由）
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _jumpCollision.IsGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }
}
