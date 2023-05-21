using UnityEngine;



/// <summary>
/// ゲーム終了時のカメラ制御
/// </summary>
public class GameEndCam : MonoBehaviour
{

    [SerializeField, Tooltip("イベント用カメラ")] private GameObject eventCamera;
    [SerializeField, Tooltip("プレイヤー")] private Transform _playerTr;
    [SerializeField, Tooltip("移動追従オブジェクト")] private Transform _targetTr;
    private bool _isResult = false;

    private void LateUpdate()
    {

        if(_isResult)
        {
            //カメラ距離を計算して旋回速度を掛け代入
            eventCamera.transform.localPosition = _targetTr.localPosition;

            //プレイヤーを直視
            eventCamera.transform.LookAt(_playerTr, Vector3.up);
        }
    }

    /// <summary>
    /// Resultカメラに変更
    /// </summary>
    public void ResultCam()
    {

        //イベントカメラをアクティブ
        eventCamera.SetActive(true);

        //Result開始
        _isResult = true;
    }
}
