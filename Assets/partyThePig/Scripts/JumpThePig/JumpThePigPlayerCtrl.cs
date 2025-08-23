using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Cysharp.Threading.Tasks;

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

    private AnimalAnimation _animalAnim;
    private bool _isAnime = false;

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
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            var playerPrefab = PlayerManager.Instance.players[playerIndex].playerSprite;
            if (playerPrefab != null)
            {


                // Prefabをこのオブジェクトの子として生成
                GameObject playerObj = Instantiate(playerPrefab, transform);

                // 色を設定（SpriteRendererがある場合）
                var renderer = playerObj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
                }
            }
            else
            {
                Debug.LogWarning($"プレイヤーPrefabが設定されていません: {playerIndex}");
            }
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }
        _animalAnim = GetComponentInChildren<AnimalAnimation>();

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
            _rb.velocity = Vector2.zero;
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

        if (_animalAnim != null && !_isAnime)
        {
            if (_moveInput != Vector2.zero)
            {
                _animalAnim.Walk(); // 入力あり → Walk
            }
            else if (context.canceled)
            {
                _animalAnim.Idle(); // 入力終了 → Idle
            }
        }
    }

    /// <summary>
    /// ジャンプ入力（Invoke Unity Events 経由）
    /// </summary>
    public async void OnJump(InputAction.CallbackContext context)
    {
        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _jumpCollision.IsGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

            if (_animalAnim != null)
            {
                _animalAnim.Jump(); // ジャンプアニメーションも再生
                _isAnime = true;
                await UniTask.Delay(1000);
                _animalAnim.Idle();
                _isAnime = false;
            }
        }
    }
}
