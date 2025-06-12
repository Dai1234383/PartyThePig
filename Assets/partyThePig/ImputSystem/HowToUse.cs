using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HowToUse : MonoBehaviour
{
    /* InputSystemの使い方
     * 
     * 1.プレイヤーのプレハブを作ってプレイヤー操作のスクリプト(例：JumpThePigPlayerCtl等)とPlayerInputコンポーネントをつける
     * 
     * 2.Create→下のほうのInputActionからInputActionを作って、名前を作るミニゲームの名前にする
     * 
     * 3.すでにあるやつを参考にするなり、ググるなりして設定する
     * (本当は２つ以上あるのよくないので、送られてきたやつをコマツが統合します)
     * 
     * 4.PlayerInputのActionsに作ったやつを入れ、BehaviorをInvokeUnityEventに変える
     * 
     * 5.Eventを開いて、さっき設定したMoveだのJumpだのがあると思うので、それにPlayerCtrlの対応するメソッドをくっつける
     * (下にメソッドの例を置いておきます)
     * (publicメソッドにして、InputAction.CallbackContext contextの部分を書かないと多分表示されないかも。分からなかったらググるか聞いてちょ)
     * (ChatGPTに投げると↑を書いてくれるかもしれないし書いてくれないかもしれない(小松の時は書いてくれなかった))
     */

    /// <summary>
    /// 移動入力
    /// </summary>
    /// <param name="context"></param>
    /*

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameState != GameManager.GameStateName.GAME) return;

        // 通常時の速度を保存
        _fastSpeed = context.ReadValue<Vector2>();
        // 燃料がない時の速度を保存
        _slowSpeed = context.ReadValue<Vector2>() * _ratioNoFuel;

        // 地面に触れている時か、天井に触っている時か、燃料があるとき入力
        if (_isGrounded || _isReverseGrounded || _currentFuel > 0)
        {
            // 入力値を保存
            _moveInput = context.ReadValue<Vector2>();
        }
        // 燃料がない時には入力値を少なくして入力
        else
        {
            _moveInput = context.ReadValue<Vector2>() * _ratioNoFuel;

            // 入力始まりだけガス欠演出
            if (context.started)
            {
                PlaySE(_gasInjectionSE);
                PlayEffect();
            }
        }

        // 移動キーが押されているかを判別
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                // 移動キーが押された
                _isPressKey = true;
                break;

            case InputActionPhase.Canceled:
                // 移動キーが離された
                _isPressKey = false;
                break;
        }

        if (Mathf.Abs(context.ReadValue<Vector2>().x) > _roundDownVectorX)
        {
            _isPressKey = true;
        }
        else
        {
            _isPressKey = false;
        }
    }

    */


}
