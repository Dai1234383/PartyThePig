using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MoneyThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("地面判定")]
    [SerializeField] private int _groundLayerNum = 6;

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private InputActionAsset _action;

    private PlayerInput _playerInput;


    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded = false;

    private AnimalAnimation _animalAnim;
    private bool _isAnime = false;

    public int PlayerIndex => playerIndex;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerIndex = _playerInput.playerIndex;  // 自動取得に変更
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // スタート位置をプレイヤーインデックスで設定
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            var playerPrefab = PlayerManager.Instance.players[playerIndex].playerSprite;
            if (playerPrefab != null)
            {
                // すでにオブジェクトがある場合は消しておく
                if (transform.childCount > 0)
                {
                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

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
        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.GAME)
        {
            Vector2 velocity = _rb.velocity;
            velocity.x = _moveInput.x * _moveSpeed;
            _rb.velocity = velocity;
        }
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;

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
    /// ジャンプ入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public async void OnJump(InputAction.CallbackContext context)
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _isGrounded)
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

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = true;
        }
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = false;
        }
    }
}
