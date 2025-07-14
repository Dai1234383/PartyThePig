using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class EscapeThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int playerIndex = -1;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private PlayerInput _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (playerIndex == -1)
        {
            Debug.LogError("PlayerIndexの取得に失敗しました。");
            return;
        }


        // 色設定
        if (PlayerManager.Instance != null)
        {
            _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        }
    }

    private void FixedUpdate()
    {
        if (EscapeThePigGameStateManager.Instance.GameState == EscapeThePigGameStateManager.GameStateName.GAME)
        {
            _rb.velocity = _moveInput * _moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;

        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// インタラクト入力（アイテム使用）
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;
        if (!context.performed) return;

        EscapeThePigPlayerItemSystem itemSystem = GetComponent<EscapeThePigPlayerItemSystem>();
        if (itemSystem != null)
        {
            itemSystem.UseItem();
        }
    }

    /// <summary>
    /// 敵に当たったらゲームオーバー
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            string winnerName = (playerIndex == 0) ? "Player2" : "Player1";
            EscapeThePigGameStateManager.Instance.GameOver(winnerName);
        }
    }

}
