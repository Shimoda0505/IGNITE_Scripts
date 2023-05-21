using UnityEngine;


/// <summary>
/// プレイヤーのアニメーション管理
/// </summary>
public class PlayerAnimator : MonoBehaviour
{

    PlayerEffect _playerEffect;//エフェクト管理スクリプト
    _InputSystemController _inputController;//入力管理スクリプト
    private Animator _anim;//プレイヤーのanimatorの取得



    //処理部--------------------------------------------------------------------------------------------------------------
    void Start()
    {
        _anim = this.gameObject.GetComponent<Animator>();//animatorの取得
        _playerEffect = this.gameObject.GetComponent<PlayerEffect>();//エフェクト管理スクリプトの取得
    }



    //アニメーション部---------------------------------------------------------------------------------------------------
    #region 羽ばたき方の変更
    [Header("羽ばたき方の変更")]
    [SerializeField, Tooltip("平行飛行の開始時間")] private float _parallelFlyTime;
    [SerializeField, Tooltip("羽ばたき飛行の開始時間")] private float _flappingFlyTime;
    [SerializeField, Tooltip("飛行変更のスピード")] private float _changeVerocitySpeed;
    [SerializeField, Tooltip("羽ばたき飛行のアニメーション最大値(0~1)")] private float _maxFlappingFly;
    private Vector2 _flyAnimTree = new Vector2(0, 0);//羽ばたきに代入(0~1)
    private bool _isFlyVertical = false;//飛行変更の遷移フラグ
    private float _changeFlyCount;//飛行変更の計測


    /// <summary>
    /// 羽ばたきと平行飛行のアニメーション切り替え
    /// </summary>
    public void ChangeFlyVerticalAnim()
    {

        //時間計測
        _changeFlyCount += Time.deltaTime;

        //羽ばたきアニメーション中
        if (!_isFlyVertical)
        {

            //平行飛行に遷移するまでの時間
            if (_changeFlyCount >= _parallelFlyTime)
            {

                //平行飛行に移行
                _flyAnimTree.y += Time.deltaTime * _changeVerocitySpeed;
                if (_flyAnimTree.y >= _maxFlappingFly)
                {

                    //トレイルエフェクト
                    _playerEffect.Trayl();

                    //平行飛行開始
                    _isFlyVertical = true;

                    //時間初期化
                    _changeFlyCount = 0;
                }
            }
        }

        //平行飛行中
        else if (_isFlyVertical)
        {

            //羽ばたき飛行に遷移するまでの時間
            if (_changeFlyCount >= _flappingFlyTime)
            {

                //羽ばたき飛行に移行
                _flyAnimTree.y -= Time.deltaTime * _changeVerocitySpeed;
                if (_flyAnimTree.y <= 0)
                {

                    //平行飛行終了
                    _isFlyVertical = false;

                    //時間初期化
                    _changeFlyCount = 0;
                }
            }
        }

        //1になった時点で羽ばたきカウントに移行
        _flyAnimTree.y = Mathf.Clamp(_flyAnimTree.y, 0, _maxFlappingFly);

        //羽ばたきと平行飛行のアニメーション(BrendTree)
        _anim.SetFloat("Vertical", _flyAnimTree.y);
    }
    #endregion


    #region 左右飛行の変更
    [Header("左右飛行の変更")]
    [SerializeField, Tooltip("左右飛行の変更速度")] private float _changeHorizontalSpeed;
    [SerializeField, Tooltip("左右飛行の戻り速度")] private float _returnHorizontalSpeed;
    [SerializeField, Tooltip("左右飛行のアニメーション最大値(0~1)")] private float _maxHorizontalFly;
    private float _animValue;//左右飛行のアニメーションに代入(0~1)


    /// <summary>
    /// 左右移動時のアニメーション切り替え
    /// </summary>
    public void ChangeFlyHorizontalAnim(float moveInputX)
    {

        //左右振りむきアニメーションを入力に合わせて動かす
        _animValue += moveInputX * Time.deltaTime * _changeHorizontalSpeed;

        //_animValueに範囲制限をつけて向きすぎないようにする
        _animValue = Mathf.Clamp(_animValue, -_maxHorizontalFly, _maxHorizontalFly);

        //左右飛行アニメーション(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);
    }

    /// <summary>
    /// 左右移動時のアニメーション戻り
    /// </summary>
    public void ReturnFlyHorizontalAnim()
    {

        //左右振りむきアニメーションを正面に戻す
        _animValue = Mathf.Lerp(_animValue, 0, Time.deltaTime * _returnHorizontalSpeed);

        //左右飛行アニメーション(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);
    }

    /// <summary>
    /// 左右移動時のアニメーション初期化
    /// </summary>
    public void ResetFlyHorizontalAnim()
    {

        //左右振りむきアニメーションを正面に戻す
        _animValue = 0;

        //左右飛行アニメーション(BrendTree)
        _anim.SetFloat("Horizontal", _animValue);
    }
    #endregion


    #region 回避
    /// <summary>
    /// 回避アニメーション
    /// </summary>
    public int DodgeAnim(float inputHorizontalValue)
    {

        //入力に応じた回避方向
        if (inputHorizontalValue >= 0)
        {
            _anim.SetBool("DoageR", true);

            //右回避の返却
            return 1;
        }
        else if (inputHorizontalValue < 0)
        {
            _anim.SetBool("DoageL", true);

            //左回避の返却
            return -1;
        }

        //返却値なし
        return 0;
    }

    /// <summary>
    /// AnimationのEventでの再生を終了
    /// </summary>
    public void DoageFalse()
    {

        _anim.SetBool("DoageR", false);
        _anim.SetBool("DoageL", false);
    }
    #endregion


    #region ダメージ
    /// <summary>
    /// ダメージ時のアニメーション
    /// </summary>
    public void DamageAnim()
    {

        //入力方向に応じてダメージアニメーションを変更
        if (_inputController.LeftStickValue().x >= 0)
        {

            _anim.SetBool("DamageR", true);
        }
        else if (_inputController.LeftStickValue().x < 0)
        {

            _anim.SetBool("DamageL", true);
        }
    }

    /// <summary>
    /// ダメージアニメーションを停止
    /// </summary>
    public void DamageFalse()
    {

        _anim.SetBool("DamageR", false);
        _anim.SetBool("DamageL", false);
    }
    #endregion


    #region 死亡,復活
    /// <summary>
    /// 死亡アニメーション
    /// </summary>
    public void DeathAnim() { _anim.SetTrigger("Death"); }

    /// <summary>
    /// 復活アニメーション
    /// </summary>
    public void ReviveAnim() { _anim.SetTrigger("ReBorn"); }
    #endregion


    #region ウルト
    /// <summary>
    /// ウルトアニメーション
    /// </summary>
    public void UltAnim() { _anim.SetTrigger("Ult"); }
    #endregion


    #region ゲームクリア
    /// <summary>
    /// ゲームクリアアニメーション
    /// </summary>
    public void GameClearAnim() { _anim.SetTrigger("GameClear"); }
    #endregion
}
