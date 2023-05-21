using UnityEngine;


/// <summary>
/// 敵が初めて画面に映った多岐にCursorで知らせる際の、Cursor挙動管理
/// </summary>
public class InViewImage : MonoBehaviour
{

    public GameObject _target = default;//追従するターゲット


    [SerializeField,Tooltip("ゲームイベント管理スクリプト")] GameSystem _gameSystem;
    [SerializeField,Tooltip("カーソルのRectTransform")] private RectTransform _rect;

    private Vector2 _rectPos = default;//カーソルの画面座標
    private Vector2 _upRect = new Vector2(0, 20);//更新座標

    private float _time = 1f;//時間
    private float _count = 0;//カウント



    //処理部-------------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //イベント中は処理しない
        if (_gameSystem.IsEvent()) { return; }

        //時間計測後に停止
        _count += Time.deltaTime;
        if(_count >= _time) { _count = 0; this.gameObject.SetActive(false); }

        //ターゲットのスクリーン座標に変換
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //スクリーン座標をRectTransfirmに変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //ターゲットの座標に移動
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }
}
