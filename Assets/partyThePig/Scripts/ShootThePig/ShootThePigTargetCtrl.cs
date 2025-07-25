using UnityEngine;

public class ShootThePigTargetCtrl : MonoBehaviour
{
    [SerializeField] private int _targetScore;

    private SpriteRenderer _spriteRenderer;
    private int _paintedPlayerIndex = -1;
    private bool _isPainted = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 的が撃たれたときに呼ばれる
    /// </summary>
    /// <param name="PlayerIndex"></param>
    public void Hit(int PlayerIndex)
    {
        //今塗っている人と撃った人が違うなら
        if (_paintedPlayerIndex != PlayerIndex)
        {
            //撃った人を登録
            _paintedPlayerIndex = PlayerIndex;
            _spriteRenderer.color = PlayerManager.Instance.players[PlayerIndex].playerColor;
            string PlayerName;
            if (PlayerIndex == 0)
            {
                PlayerName = "Player1";
            }
            else
            {
                PlayerName = "Player2";
            }

            //スコアの変動を呼ぶ
            ShootThePigScoreManager.Instance.AddScore(PlayerName, _targetScore, _isPainted);

            //塗られているように更新
            _isPainted = true;
        }

    }
}
