using UnityEngine;



/// <summary>
/// Cursorの座標を移動させる
/// </summary>
public class CursorController : MonoBehaviour
{

    public GameObject _target = default;//追従するターゲット
    [SerializeField,Tooltip("カーソルの画像管理スクリプト")] PointerImage _pointerImage;
    [SerializeField,Tooltip("ゲームシステムスクリプト")] GameSystem _gameSystem;
    [SerializeField,Tooltip("CanvasTransform")] private RectTransform _rect;
    private Vector2 _rectPos = default;//スクリーン座標
    private Vector2 _upRect = new Vector2(0, 20);



    //処理-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {

        //ターゲットがBullになった時
        if(_target ==null)
        {

            //カーソルのカラー変更
            _pointerImage.DefaultColorChange();

            //カーソルのアクティブを消す
            this.gameObject.SetActive(false);

            return;
        }

        //イベント中は処理しない
        if (_gameSystem.IsEvent()) { return; }

        //ターゲットのスクリーン座標に変換
        Vector2 enemyPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);

        //スクリーン座標をRectTransfirmに変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, enemyPos, Camera.main, out _rectPos);

        //ターゲットの座標に移動
        this.gameObject.GetComponent<RectTransform>().localPosition = _rectPos + _upRect;
    }
}
