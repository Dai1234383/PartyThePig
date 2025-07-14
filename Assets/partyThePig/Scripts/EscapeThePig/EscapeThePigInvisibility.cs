using UnityEngine;

/// <summary>
/// プレイヤーが一時的に敵から見えなくなるフラグ管理
/// </summary>
public class EscapeThePigInvisibility : MonoBehaviour
{
    public bool IsInvisible { get; private set; }

    /// <summary>
    /// 外部から呼び出して不可視状態をON/OFFにする
    /// </summary>
    public void SetInvisible(bool state)
    {
        IsInvisible = state;
    }
}
