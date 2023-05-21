using UnityEngine;



/// <summary>
/// ゲーム全体のイベント中かを管理
/// </summary>
public class GameSystem : MonoBehaviour
{

    //イベント管理フラグ
    private bool _isEvent;



    //処理部-------------------------------------------------------------------------------------------------------
    //カーソルを消す
    private void Start()
    {
        Cursor.visible = false;
    }


    //メソッド部----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Eventの終了
    /// </summary>
    public void FalseIsEvent() { _isEvent = false; }

    /// <summary>
    /// Eventの開始
    /// </summary>
    public void TrueIsEvent() { _isEvent = true; }

    /// <summary>
    /// イベント状態の返却
    /// </summary>
    public bool IsEvent() { return _isEvent; }
}
