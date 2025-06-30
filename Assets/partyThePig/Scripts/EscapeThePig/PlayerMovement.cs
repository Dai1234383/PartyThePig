using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;    //画像

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;  // 自動取得に変更

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Start()
    {
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
