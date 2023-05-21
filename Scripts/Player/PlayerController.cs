using UnityEngine;


/// <summary>
/// プレイヤーの井戸と旋回を管理
/// </summary>
public class PlayerController : MonoBehaviour
{

    #region public
    //プレーヤーの状態
    public PlayerMotion _playerMotion = PlayerMotion.Fly;
    public enum PlayerMotion
    {
        Fly,//飛行
        Dodge,//回避
        Damage,//ダメージ
        Death,//死亡
        Revive,//復活
    }
    public int _reverseInput = -1;//入力の反転キーコン用
    /// <summary>
    /// プレイヤーの左右に動く範囲
    /// </summary>
    public Vector3 MoveClamp() { return _moveClamp; }
    /// <summary>
    /// プレイヤーが実際に移動した量
    /// </summary>
    public Vector3 InputMove() { return _inputMove; }
    /// <summary>
    /// 回避後の移動方向
    /// </summary>
    public float DoageDirection() { return _doageDirection; }
    /// <summary>
    /// ウルト使用中か
    /// </summary>
    public bool IsUlt() { return _isUlt; }
    /// <summary>
    /// スキル使用後の時間計測
    /// </summary>
    public Vector3 SkillCount() { return _skillCount; }
    /// <summary>
    /// スキル使用後のインターバル
    /// </summary>
    public Vector3 SkillTime() { return _skillTime; }
    /// <summary>
    /// 無敵フラグ
    /// </summary>
    public bool IsInvincible() { return _isInvincible; }
    #endregion

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーのアニメーション管理")] PlayerAnimator _playerAnimator;
    [SerializeField, Tooltip("ゲームのイベントやTime.deltaTimeを管理")] GameSystem _gameSystem;
    [SerializeField, Tooltip("プレイヤーのステータス")] PlayerStatus _playerStatus;
    [SerializeField, Tooltip("プレイヤースプライン")] PlayerMoveSpline _playerMoveSpline;
    [SerializeField, Tooltip("Cursor変更スクリプト")] PointerChange _pointerChange;
    [SerializeField, Tooltip("ボスステータス管理スクリプト")] BossStatus _bossStatus;
    [SerializeField, Tooltip("プレイヤーのse管理スクリプト")] PlayerSE _playerSE;
    [SerializeField, Tooltip("プレイヤーのエフェクト管理スクリプト")] PlayerEffect _playerEffect;
    _InputSystemController _inputController;//入力周り
    AnyUseMethod _anyUseMethod;//よく使うメソッド
    #endregion

    #region 傾き
    [Header("傾き")]
    [SerializeField, Tooltip("移動角度の速度")] private Vector3 _rotateSpeed;
    [SerializeField, Tooltip("移動角度戻り時の速度")] private Vector3 _reRotateSpeed;
    [SerializeField, Tooltip("移動角度Clamp")] private Vector3 _rotateClamp;
    private Vector3 _inputRotate = default;//本体の傾き
    #endregion

    #region 移動
    [Header("移動")]
    [SerializeField, Tooltip("移動距離の速度")] private Vector2 _moveSpeed;
    [SerializeField, Tooltip("移動距離Clamp")] private Vector3 _moveClamp;
    private Vector3 _moveInput = default;//左スティックの入力
    private Vector3 _inputMove = default;//移動速度
    #endregion

    #region 回避
    [Header("回避")]
    [SerializeField, Tooltip("回避終了の時間")] private float _doageTime;
    [SerializeField, Tooltip("回避後の移動量")] private float _doageMove;
    [SerializeField, Tooltip("回避の移動速度")] private float _doageSpeed;
    private float _doageCount = 0;//回避終了の時間計測
    private float _doageDirection = default;//回避後の移動方向
    private float _doagePos = default;//回避前の位置に移動量の加算(実移動位置)
    #endregion

    #region スキル
    [Header("スキル")]
    [SerializeField, Tooltip("スキル使用後のインターバル(回復/バリア/ウルト)")] private Vector3 _skillTime;
    private bool _isHealing = false;//回復したか
    private bool _isBarrier = false; //バリア使用したか
    private bool _isUlt = false;//ウルト使用したか
    private Vector3 _targetPos = default;//ウルト使用時の敵位置
    private Vector3 _skillCount = default;//スキル使用後の時間計測
    #endregion

    #region ダメージ
    [Header("ダメージ")]
    [SerializeField, Tooltip("ダメージ後のインターバル")] private float _damageTime;
    [SerializeField, Tooltip("ダメージ後の無敵時間")] private float _invincibleTime;    
    private float _damageCount = 0;//ダメージ後の計測
    private bool _isInvincible = false;//無敵フラグ
    #endregion

    #region 死亡、復活
    [Header("死亡、復活")]
    [SerializeField, Tooltip("復活から開始までの時間")] private float _reviveTime;
    private float _reviveCount = default;//復活から開始までの時間を計測
    #endregion



    //処理-------------------------------------------------------------------------------------------------
    private void Start()
    {

        //スキル時間の初期化
        _skillCount = _skillTime;
    }

    private void Update()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem.IsEvent()) { return; }

        switch (_playerMotion)
        {

            case PlayerMotion.Fly:

                //移動の入力
                MoveInput();

                //回避の入力
                DodgeInput();

                //スキルの入力
                SkillInput();
                break;


            case PlayerMotion.Death:

                break;
        }
    }


    private void FixedUpdate()
    {

        //時間0もしくは,イベント中なら何もしない
        if (Time.timeScale == 0 || _gameSystem.IsEvent())
        {
            //ターゲットを直視,Ult時
            if (_gameSystem.IsEvent() && _isUlt && _bossStatus._motion != BossStatus.Motion.END)
            {

                GameObject target = _playerStatus.UltTarget();

                if (target.tag == "Enemy") { this.gameObject.transform.LookAt(_targetPos); }
                else if (target.tag == "Boss") { this.gameObject.transform.LookAt(target.transform); }

            }
            //ゲームクリア
            else
            {

                //本体を(Xの入力値,Yの入力値,Zの傾き)に回転
                this.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

                //羽ばたきと平行飛行のアニメーション切り替え
                _playerAnimator.ChangeFlyVerticalAnim();

                //左右移動時のアニメーションを基準値に戻す
                _playerAnimator.ReturnFlyHorizontalAnim();
            }

            return;
        }

        switch (_playerMotion)
        {

            //飛行
            case PlayerMotion.Fly:

                //羽ばたきと平行飛行のアニメーション切り替え
                _playerAnimator.ChangeFlyVerticalAnim();

                //プレイヤーの移動計算
                MoveProcess();

                //プレーヤーの移動を実行.反映
                MoveExecution();
                break;


            //回避
            case PlayerMotion.Dodge:

                //プレイヤーの回避移動の計算
                DoageProcess();

                //プレーヤーの移動を実行.反映
                MoveExecution();
                break;


            //ダメージ
            case PlayerMotion.Damage:

                //ダメージの処理
                Damage();

                //プレーヤーの移動を実行.反映
                MoveExecution();
                break;


            //死亡
            case PlayerMotion.Death:

                break;


            //復活
            case PlayerMotion.Revive:

                //復活の処理
                Revive();
                break;
        }


        //無敵時間
        Invincible();

        //スキル使用後のインターバル
        ShillResetting();
    }



    //privateメソッド群--------------------------------------------------------------------------------

    //入力
    /// <summary>
    /// 移動の入力
    /// </summary>
    private void MoveInput()
    {

        //左スティックの入力している時
        if (_inputController.LeftStick())
        {

            //入力量が0.1を超えているなら(誤動作防止用)
            if (Mathf.Abs(_inputController.LeftStickValue().x) >= 0.1f || Mathf.Abs(_inputController.LeftStickValue().y) >= 0.1f)
            {
                //入力に斜めの入力を正規化
                _moveInput = _anyUseMethod.InputNomarizeVector2(_inputController.LeftStickValue(), _reverseInput * -1);

                //左右移動時のアニメーション切り替え
                _playerAnimator.ChangeFlyHorizontalAnim(_moveInput.x);
            }
        }

        //左スティックの入力していない時
        else if (!_inputController.LeftStick())
        {

            //左右移動時のアニメーションを基準値に戻す
            _playerAnimator.ReturnFlyHorizontalAnim();
        }
    }

    /// <summary>
    /// スキル使用後のインターバル
    /// </summary>
    private void ShillResetting()
    {

        //回復
        if (_isHealing)
        {

            //時間計測後回復false
            _skillCount.x += Time.deltaTime;
            if (_skillCount.x >= _skillTime.x) { _isHealing = false; }
        }

        //バリア
        if (_isBarrier)
        {

            //時間計測後バリアfalse
            _skillCount.y += Time.deltaTime;
            if (_skillCount.y >= _skillTime.y) { _isBarrier = false; }
        }

        //ウルト
        if (_isUlt)
        {

            //時間計測後
            _skillCount.z += Time.deltaTime;
            if (_skillCount.z >= _skillTime.z)
            {

                //ウルトfalse
                _isUlt = false;

                //レティクルの変更
                _pointerChange.ChangeRet();
            }
        }
    }

    /// <summary>
    /// スキルの使用
    /// </summary>
    private void SkillInput()
    {

        //Xボタンの入力
        if (_inputController.ButtonWestDown() && !_isHealing)
        {

            //回復の使用とスキル使用フラグtrue
            _isHealing = _playerStatus.UseSkill("回復");
            if (_isHealing)
            {

                //回復SE
                _playerSE.HealSe();

                //時間初期化
                _skillCount.x = 0;
            }
        }

        //Bボタンの入力
        else if (_inputController.ButtonEastDown() && !_isBarrier)
        {

            //バリアの使用とスキル使用フラグtrue
            _isBarrier = _playerStatus.UseSkill("バリア");

            if (_isBarrier) { _skillCount.y = 0; }
        }

        //Yボタンの入力
        else if (_inputController.ButtonNorthDown() && !_isUlt)
        {

            //ウルトの使用とスキル使用フラグtrue
            _isUlt = _playerStatus.UseSkill("ウルト");

            if (_isUlt)
            {

                //時間初期化
                _skillCount.z = 0;

                //ターゲットがNullかどうか
                if (_playerStatus.UltTarget() != null) { _targetPos = _playerStatus.UltTarget().transform.position; }
            }
        }
    }


    //飛行
    /// <summary>
    /// プレイヤーの移動計算
    /// </summary>
    private void MoveProcess()
    {

        //左スティックの入力
        if (_inputController.LeftStick())
        {

            //角度処理
            //入力に制限をつけて角度加算
            _inputRotate = _anyUseMethod.MoveClampVector3(_inputRotate, new Vector3(-_moveInput.y, _moveInput.x, -_moveInput.x), _rotateSpeed, -_rotateClamp, _rotateClamp);

            //移動処理
            //入力に制限をつけて移動
            _inputMove = _anyUseMethod.MoveClampVector3(_inputMove, _moveInput, _moveSpeed, -_moveClamp, _moveClamp);
        }
        //左スティックの入力なし
        else if (!_inputController.LeftStick())
        {

            //入力値の初期化
            _moveInput = new Vector2(0, 0);

            //入力値を初期値に戻す(始点,終点,速度)
            _inputRotate = _anyUseMethod.MoveToWardsAngleVector3(_inputRotate, new Vector3(0, 0, 0), Time.deltaTime * _reRotateSpeed);
        }
    }

    /// <summary>
    /// プレーヤーの移動を実行.反映
    /// </summary>
    private void MoveExecution()
    {

        //本体の位置移動
        this.gameObject.transform.localPosition = new Vector3(_inputMove.x, _inputMove.y, 0);

        //本体を(Xの入力値,Yの入力値,Zの傾き)に回転
        this.gameObject.transform.localEulerAngles = new Vector3(_inputRotate.x, _inputRotate.y, _inputRotate.z);
    }


    //回避
    /// <summary>
    /// 回避の入力
    /// </summary>
    private void DodgeInput()
    {

        //Aボタンを押すと回避
        if (_inputController.ButtonSouthDown())
        {

            //回避アニメーションの再生と回避方向の返却
            //最後に入力した方向に回避
            _doageDirection = _playerAnimator.DodgeAnim(_inputController.LeftStickValue().x);

            //回避前の位置に移動量を加算(実移動位置)
            _doagePos = _inputMove.x + _doageMove * _doageDirection;

            //回避SE
            _playerSE.BrinkSe();

            //回避エフェクト
            _playerEffect.Doage();

            //回避Enumに遷移
            _playerMotion = PlayerMotion.Dodge;
        }
    }

    /// <summary>
    /// プレイヤーの回避移動の計算
    /// </summary>
    private void DoageProcess()
    {

        //回避後の移動
        _inputMove.x = _anyUseMethod.LerpClampVector3(_inputMove, new Vector3(_doagePos, 0, 0), new Vector3(_doageSpeed, 0, 0), -_moveClamp, _moveClamp).x;

        //時間の計測
        _doageCount += Time.deltaTime;

        //時間がたったらFly(Enum)に戻る
        if (_doageCount >= _doageTime)
        {

            //時間初期化
            _doageCount = 0;

            //飛行Enumに遷移
            _playerMotion = PlayerMotion.Fly;
        }
    }


    //ダメージ
    /// <summary>
    /// ダメージの処理
    /// </summary>
    private void Damage()
    {

        //時間の計測
        _damageCount += Time.deltaTime;

        if (_damageCount >= _damageTime)
        {

            //ダメージの時間を初期化
            _damageCount = 0;

            //ダメージアニメーションを停止
            _playerAnimator.DamageFalse();

            //無敵フラグ
            _isInvincible = true;

            //飛行Enumに遷移
            _playerMotion = PlayerMotion.Fly;
        }
    }

    /// <summary>
    /// 無敵時間
    /// </summary>
    private void Invincible()
    {

        if (_isInvincible)
        {

            //時間の計測
            _damageCount += Time.deltaTime;
            if (_damageCount >= _invincibleTime)
            {

                //ダメージの時間を初期化
                _damageCount = 0;

                //無敵フラグ
                _isInvincible = false;
            }
        }
    }


    //死亡
    /// <summary>
    /// 復活の処理
    /// </summary>
    private void Revive()
    {

        //時間計測
        _reviveCount += Time.deltaTime;

        //時間経過したら
        if (_reviveCount >= _reviveTime)
        {

            //時間を初期化
            _reviveCount = 0;

            //移動開始
            _playerMoveSpline.ChangeMoveSpeed("移動");

            //移動Enumに遷移
            _playerMotion = PlayerMotion.Fly;
        }
    }


    //publicメソッド群--------------------------------------------------------------------------------

    /// <summary>
    /// 死亡開始の処理
    /// </summary>
    public void DeathStart()
    {

        //移動停止
        _playerMoveSpline.ChangeMoveSpeed("停止");

        //死亡アニメーション再生
        _playerAnimator.DeathAnim();

        //プレイヤーのEnumを死亡に遷移
        _playerMotion = PlayerMotion.Death;
    }
}
