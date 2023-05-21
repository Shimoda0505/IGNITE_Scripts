using UnityEngine;



/// <summary>
/// ステージのLighting変更
/// </summary>
public class ChangeLighting : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField,Tooltip("プレイヤーのルートイベント管理スクリプト")] RootNav _rootNav;
    AnyUseMethod _anyUseMethod;
    #endregion


    #region 環境光設定
    [Header("環境光設定")]
    [SerializeField, Tooltip("環境光の色")] private Color _changeColor;
    [SerializeField, Tooltip("色の変わる速度")] private float _colorSpeed;
    private Color _defColor = default;//初期の環境光の色
    private Color _color = default;//環境光の保管
    #endregion


    #region フォグ
    [Header("フォグ")]
    [SerializeField, Tooltip("フォグの距離")] private float _changeFogDensity;
    [SerializeField,Tooltip("フォグの速度")] private float _fogSpeed;
    private float _defFogDensity = default;//初期のフォグの距離
    private float _fog = default;//フォグの保管
    #endregion


    #region 時間
    [Header("時間")]
    [SerializeField, Tooltip("時間")] private float _time;
    private float _count = 0;//時間カウント
    #endregion


    #region ルート
    [Header("ルート関連")]
    [SerializeField, Tooltip("変更ポイント")] private GameObject[] _changePoints;
    private int _number = 0;//変更番号
    #endregion


    Motion _motion = Motion.WAIT;//変更enum
    enum Motion
    {
        
        WAIT,
        CHANGE
    }



    //処理部------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //初期の環境光の色
        _defColor = RenderSettings.ambientSkyColor;

        //初期のフォグの距離
        _defFogDensity = RenderSettings.fogDensity;
    }

    private void FixedUpdate()
    {


        switch (_motion)
        {

            case Motion.WAIT:

                //番号オブジェクトなら処理
                if (_rootNav.NowPoint() == _changePoints[_number])
                {

                    //変更
                    if (_number == 0) { Change(_changeColor, _changeFogDensity); }

                    //初期化
                    else if (_number == 1) { Change(_defColor, _defFogDensity);}

                    //ルート番号の更新
                    _number++;
                }
                break;


            case Motion.CHANGE:

                //光の変更
                RenderSettings.ambientSkyColor = _anyUseMethod.MoveToWardsColorVector3(RenderSettings.ambientSkyColor, _color, _colorSpeed);

                //フォグの変更
                RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, _fog, _fogSpeed);

                //時間経過後に処理
                _count += Time.deltaTime;
                if (_count >= _time)
                {

                    //環境光の変更
                    if(_number > _changePoints.Length - 1) { this.gameObject.GetComponent<ChangeLighting>().enabled = false; }

                    //初期化
                    _count = 0;
                    _motion = Motion.WAIT;
                }
                break;
        }
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 変更(色 / 距離)
    /// </summary>
    private void Change(Color color,float density) { _color = color; _fog = density; _motion = Motion.CHANGE; }

}
