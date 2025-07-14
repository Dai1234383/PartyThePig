using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _collision;



    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private JumpThePigCollision _JumpThePigCollision;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _JumpThePigCollision = _collision.GetComponent<JumpThePigCollision>();
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
        if (JumpThePigGameStateManager.Instance.GameState == JumpThePigGameStateManager.GameStateName.GAME)
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
        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME) return;

        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ジャンプ入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (JumpThePigGameStateManager.Instance.GameState != JumpThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _JumpThePigCollision.IsGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

    
}
