using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoneyThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("地面判定")]
    [SerializeField] private int _groundLayerNum = 6;

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private PlayerInput input;


    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded = false;

    public int PlayerIndex => playerIndex;

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
    }


    private void FixedUpdate()
    {
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
    }

    /// <summary>
    /// ジャンプ入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
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
