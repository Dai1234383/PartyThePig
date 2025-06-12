using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("地面判定")]
    [SerializeField] private int _groundLayerNum = 6;

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定

    private PlayerInput input;


    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        Debug.Log($"This is player {playerIndex}");
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(_isGrounded);
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

        if (context.performed && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = false;
        }
    }

}
