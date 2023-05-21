using UnityEngine;


/// <summary>
/// ボス戦時のカットイン管理
/// </summary>
public class CutInStart : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーのルートイベント管理スクリプト")] RootNav _rootNav;
    [SerializeField, Tooltip("カットイン管理スクリプト")] Cutin _cutin;/*【他メンバーが制作したため添付してません】*/
    [SerializeField, Tooltip("プレイヤーのSpline移動管理スクリプト")] PlayerMoveSpline _playerMoveSpline;
    [SerializeField, Tooltip("ゲームイベント管理スクリプト")] GameSystem _gameSystem;
    [SerializeField, Tooltip("ボスステータス管理スクリプト")] BossStatus _bossStatus;
    [SerializeField, Tooltip("ゲームBGM管理スクリプト")] PlayerBgm _playerBgm;
    #endregion


    #region その他
    [Header("その他")]
    [SerializeField, Tooltip("開始位置")] private GameObject _startPosObj;
    [SerializeField, Tooltip("移動開始までの時間")] private float _time;
    private float _count = 0;//カットイン時間のカウント
    #endregion


     Motion _motion = Motion.WAIT;//カットインenum
    enum Motion
    {
        WAIT,
        EVENT
    }



    //処理部-----------------------------------------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        switch (_motion)
        {

            case Motion.WAIT:

                if (_rootNav.NowPoint() == _startPosObj)
                {

                    //移動停止
                    _playerMoveSpline.ChangeMoveSpeed("停止");

                    //イベント開始
                    _gameSystem.TrueIsEvent();

                    //BGM変更
                    _playerBgm.SecondBgm();

                    //カットインの開始
                    _cutin.RaizoCutIn();

                    _motion = Motion.EVENT;
                }
                break;


            case Motion.EVENT:

                //時間経過後移動
                _count += Time.deltaTime;
                if(_count >= _time) { _playerMoveSpline.ChangeMoveSpeed("移動"); }

                //カットイン中なら
                if(_cutin.IsCutIn())
                {

                    //初期化
                    _count = 0;

                    //ボス戦闘開始
                    _bossStatus.IsStart();

                    //イベント終了
                    _gameSystem.FalseIsEvent();

                    //アクティブ終了
                    this.gameObject.SetActive(false);
                }

                break;
        }
    }
}
