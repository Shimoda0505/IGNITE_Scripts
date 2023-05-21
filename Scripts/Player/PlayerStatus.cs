using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// プレイヤーのステータス管理
/// </summary>
public class PlayerStatus : MonoBehaviour
{

    #region パブリック
    /// <summary>
    /// 残機
    /// </summary>
    public int PlayerRemaining() { return _playerRemaining; }
    /// <summary>
    /// バリア残量
    /// </summary>
    public float ShildVolume() { return _playerShild / _maxShild; }
    /// <summary>
    /// 最大スキル量
    /// </summary>
    public float MaxSkillVol() { return _maxSkillVol; }
    /// <summary>
    /// //現在のスキル量
    /// </summary>
    public float SkillVol() { return _skillVol; }
    /// <summary>
    /// //スキルが最大値かどうか
    /// </summary>
    public bool IsSkillMax() { return _isSkillMax; }
    #endregion


    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーのコントローラー")] PlayerController _playerController;
    [SerializeField, Tooltip("プレイヤーアニメーション")] PlayerAnimator _playerAnimator;
    [SerializeField, Tooltip("ロックオンのシステム")] PointerLockOn _pointerLockOn;
    [SerializeField, Tooltip("ウルトのカットシーン")] GENSHIN_CutIn _cutIn;/*【他メンバーが制作したため添付してません】*/
    [SerializeField, Tooltip("チェインUI")] ComboSystem _comboSystem;/*【他メンバーが制作したため添付してません】*/
    [SerializeField, Tooltip("エフェクトコントローラー")] PlayerEffect _playerEffect;
    [SerializeField, Tooltip("スコア管理スクリプト")] ScoreManager _scoreManager;
    [SerializeField, Tooltip("コンテニュー管理スクリプト")] ContinueController _continueController;/*【他メンバーが制作したため添付してません】*/
    [SerializeField, Tooltip("Cursorの変更管理スクリプト")] PointerChange _pointerChange;
    [SerializeField, Tooltip("敵管理スクリプト")] PlayerLinkEnemy _playerLinkEnemy;
    [SerializeField, Tooltip("ゲームイベント管理スクリプト")] GameSystem _gameSystem;
    [SerializeField, Tooltip("オブジェクトプール")] PoolManager _poolManager;
    PoolController _ultPool;
    #endregion


    #region HP
    [Header("HP")]
    [SerializeField, Tooltip("初期HP")] private float _playerHp;    
    private float _maxHp = default;//最大Hp
    [SerializeField, Tooltip("残機")] private int _playerRemaining;
    [SerializeField, Tooltip("ダメージパネル")] private GameObject _damagePanel;
    #endregion


    #region バリア
    [Header("バリア")]
    [SerializeField, Tooltip("最大バリア量")] private float _maxShild;
    private float _playerShild = default;//バリア残量
    [SerializeField, Tooltip("バリア消え始めの時間")] private float _startBariaoutTime;
    private float _startBariaOutCount = 0;//バリア消え始めの時間をカウント
    #endregion


    #region ゲージ
    [Header("ゲージ")]
    [SerializeField, Tooltip("ロックオンのUi")] private GameObject _lockOnUi;
    [SerializeField, Tooltip("HpのUi")] private Image _hpImage;
    [SerializeField, Tooltip("SkillのUi")] private Image _skillImage;
    [SerializeField, Tooltip("Hpマテリアル")] private Material _hpM;
    [SerializeField, Tooltip("Skillマテリアル")] private Material _skillM;
    private int _fillAmount = default;//マテリアルID
    #endregion


    #region スキル
    [Header("スキル")]
    [SerializeField, Tooltip("最大スキル量")] private float _maxSkillVol;
    [SerializeField, Tooltip("回復量")] private int _heelVolume;
    [SerializeField, Tooltip("ウルトの発射位置")] private Transform _shotPos;
    private GameObject _ultTarget = default;//ウルトのターゲットオブジェクト
    private float _skillVol = 0;//現在のスキル量
    private bool _isSkillMax = false;//スキルが最大値かどうか
    #endregion


    #region ロックオン
    [Header("ロックオン")]
    [SerializeField, Tooltip("最大ロックオン数")] private int _maxLock;
    [SerializeField, Tooltip("レティクルの変更値")] private Vector2 _changeLock;
    private int _lockCount = default;//現在のロックオン数
    #endregion


    #region 変数
    [Header("チェイン")]
    [SerializeField, Tooltip("スキルの加算倍率UI")] private Text _skillText;
    [SerializeField, Tooltip("スキルの加算倍率UIの最大フォント")] private int _maxChainFontSize;
    [SerializeField, Tooltip("チェイン倍率")] private Vector2 _chainMag;
    private int _chainCount = 0;//チェイン数
    private int _defChainFontSize = default;//スキルの加算倍率UIのデフォルトフォント
    #endregion


    #region　死亡
    [Header("死亡関連")]
    [SerializeField, Tooltip("ルートナビオブジェクト")] private GameObject _rootNav;
    [SerializeField, Tooltip("死亡時に落下するＹ座標")] private float _fallPos;
    [SerializeField, Tooltip("落下速度")] private float _fallSpeed;
    [SerializeField, Tooltip("戻り速度")] private float _reviveSpeed;
    [SerializeField, Tooltip("死亡後のインターバル")] private float _deathIntervalTime;
    private float _deathIntervalCount = 0;//死亡後のインターバルをカウント
    private bool _isDeath = false;//死亡フラグ    
    private float _deathPosY = default;//死亡時の位置補完
    DeathMotion _deathMotion = DeathMotion.Wait;//死亡後のMotion
    enum DeathMotion
    {
        Wait,
        Death,
        ReBorn
    }
    #endregion



    //処理部-----------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //シェーダーIDの取得
        _fillAmount = Shader.PropertyToID("_FillAmount");

        //スキルの加算倍率UIのデフォルトフォント
        _defChainFontSize = _comboSystem.MathSize;

        //最大Hpの設定
        _maxHp = _playerHp;

        //プレイヤーのプール管理からプール設定
        for (int i = 0; i <= _poolManager._poolArrays.Length - 1; i++)
        {

            //プール名を取得
            string poolName = _poolManager._poolArrays[i]._poolName;

            //スクリプトを取得
            PoolController poolScript = _poolManager._poolArrays[i]._poolControllers;

            //名前一致のプールを探索かける
            if (poolName == "必殺技") { _ultPool = poolScript; }
        }
    }

    private void FixedUpdate()
    {

        //UI----------------------------------------------------------------------------
        //スキル量変更
        if (_skillVol >= _maxSkillVol) { _isSkillMax = true; }
        else { _isSkillMax = false; }

        //Hpバー変更
        _hpImage.fillAmount = _playerHp / _maxHp;
        _hpM.SetFloat(_fillAmount, _playerHp / _maxHp);

        //Skillバー変更
        _skillImage.fillAmount = _skillVol / _maxSkillVol;
        _skillM.SetFloat(_fillAmount, _skillVol / _maxSkillVol);


        //Effect-------------------------------------------------------------------------
        //バリアを時間経過後に消す
        if (_playerShild > 0)
        {

            //時間計測
            _startBariaOutCount += Time.deltaTime;

            //時間計測後
            if (_startBariaOutCount >= _startBariaoutTime) { _playerShild -= Time.deltaTime * 200; }
        }


        //チェイン-----------------------------------------------------------------------
        //チェイン加算量
        if (_chainCount < _chainMag.x) { _skillText.text = "×" + 1; }
        else if (_chainMag.y > _chainCount && _chainCount >= _chainMag.x) { _skillText.text = "×" + 2; }
        else if (_chainCount >= _chainMag.y) { _skillText.text = "×" + 3; }

        //ロックオン数に応じてレティクルの変更
        LockChange();


        //死亡---------------------------------------------------------------------------
        //死亡時の座標
        switch (_deathMotion)
        {

            case DeathMotion.Wait:

                //死亡後の無敵時間
                if (_isDeath)
                {
                    //時間経過後に無敵を終了
                    _deathIntervalCount += Time.deltaTime;
                    if (_deathIntervalCount >= _deathIntervalTime) { _isDeath = false; }
                }
                break;


            case DeathMotion.Death:


                if (_rootNav.transform.localPosition.y >= _fallPos)
                {

                    //落下
                    _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                                   _rootNav.transform.localPosition.y - Time.deltaTime * _fallSpeed,
                                                                   _rootNav.transform.localPosition.z);
                }
                break;


            case DeathMotion.ReBorn:

                //復活
                _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                               _rootNav.transform.localPosition.y + Time.deltaTime * _reviveSpeed,
                                                               _rootNav.transform.localPosition.z);

                if (_rootNav.transform.localPosition.y >= _deathPosY)
                {

                    _rootNav.transform.localPosition = new Vector3(_rootNav.transform.localPosition.x,
                                                               _deathPosY,
                                                               _rootNav.transform.localPosition.z);
                    _deathMotion = DeathMotion.Wait;
                }
                break;
        }
    }



    //メソッド部-------------------------------------------------------------------------------------------------------------------------------------------------
    //HP-------------------------------------------------------------------------------------------
    /// <summary>
    /// ダメージ処理
    /// それ以降の処理が必要がないタイミングでreturn
    /// </summary>
    public bool Hit(int damage)
    {

        //飛行Enum以外の時 || 無敵時間
        if (_playerController._playerMotion != PlayerController.PlayerMotion.Fly || _playerController.IsInvincible() || _gameSystem.IsEvent()) { return false; }

        //ウルト使用中なら
        if (_playerController.IsUlt())
        {

            //回復
            _playerHp += damage;

            //最大値を超えたら
            if (_playerHp >= _maxHp) { _playerHp = _maxHp; }

            //回復エフェクト
            _playerEffect.Heal();

            return true;
        }

        //バリア量が残っているなら
        if (_playerShild > 0)
        {

            //バリアにダメージ
            _playerShild -= damage;

            //バリア破壊エフェクト
            _playerEffect.ShildBreak();

            return true;
        }

        //Hpにダメージ
        else if ((_playerShild <= 0))
        {

            //ダメージを受けたときにロックオン配列を探索してロックオン解除
            _pointerLockOn.DamageResetting();

            //チェインの初期化
            ChainResetting();

            //ロックオン数の初期化
            LockPrice("初期化");

            //Hpの減算
            _playerHp = _playerHp - damage;

            _damagePanel.SetActive(true);

            //Hpが0以下なら
            if (_playerHp <= 0)
            {

                //死亡フラグ
                _isDeath = true;

                //残機0以下ならゲームオーバー
                if (_playerRemaining <= 0) { _continueController.GameOver(); return true; }

                _deathPosY = this.gameObject.transform.localPosition.y;

                //死亡Enum
                _deathMotion = DeathMotion.Death;

                //死亡と直後の処理
                _playerController.DeathStart();

                //コンテニューUI
                _continueController.IsDeath();

                _lockOnUi.SetActive(false);

                return true;
            }

            //ダメージアニメーション再生
            _playerAnimator.DamageAnim();

            //プレイヤーのEnumをダメージに遷移
            _playerController._playerMotion = PlayerController.PlayerMotion.Damage;
        }

        return true;
    }


    //死亡-----------------------------------------------------------------------------------------
    /// <summary>
    /// 復活開始の処理
    /// </summary>
    public void ReviveStart()
    {

        //復活Enum
        _deathMotion = DeathMotion.ReBorn;

        //残機減算
        _playerRemaining--;

        //Hpを最大値に変更
        _playerHp = _maxHp;

        //復活アニメーション再生
        _playerAnimator.ReviveAnim();

        //復活Enumに遷移
        _playerController._playerMotion = PlayerController.PlayerMotion.Revive;

        //ロックオンUIをアクティブ
        _lockOnUi.SetActive(true);
    }


    //スキル-----------------------------------------------------------------------------------------
    /// <summary>
    /// スキルの使用(回復/バリア/ウルト)
    /// </summary>
    public bool UseSkill(string skill)
    {

        //回復 && 回復数が1回以上
        if (skill == "回復" && _skillVol >= _maxSkillVol / 4)
        {

            //スキルの減算
            _skillVol -= _maxSkillVol / 4;

            //回復
            _playerHp += _heelVolume;

            //最大値を超えたら
            if (_playerHp >= _maxHp) { _playerHp = _maxHp; }

            //回復エフェクト
            _playerEffect.Heal();

            return true;
        }

        //バリア && バリア回数が1回以上
        else if (skill == "バリア" && _skillVol >= _maxSkillVol / 2)
        {

            //スキルの減算
            _skillVol -= _maxSkillVol / 2;

            //バリア量を最大値に変更
            _playerShild = _maxShild;

            //シールドのアクティブ
            _playerEffect.Shild();

            //バリア時間の初期化
            _startBariaOutCount = 0;

            return true;
        }

        //ウルト && ウルト使用回数が1回以上
        else if (skill == "ウルト" && _skillVol >= _maxSkillVol)
        {

            //スキルの減算
            _skillVol -= _maxSkillVol;

            //敵の格納配列を探索
            for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
            {

                //敵の補完配列から1体を補完
                PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];

                //ロックオン中 && 弾が打たれていない敵がいるなら
                if (targets._isLock && targets._fireBoll == null)
                {

                    _ultTarget = targets._targetObj;

                    //ウルトのカットシーン
                    _cutIn.CutIn();

                    break;
                }
            }

            //ロックオン数の初期化
            LockPrice("初期化");

            //レティクルの変更
            _pointerChange.ChangeRet();

            return true;
        }

        return false;
    }

    /// <summary>
    /// ウルト使用時のターゲット
    /// </summary>
    public GameObject UltTarget() { return _ultTarget; }

    /// <summary>
    /// アニメーションEventsで使用
    /// </summary>
    public void UltShot()
    {

        //オブジェクトプールから呼び出し
        GameObject obj = _ultPool.GetObj();

        //アクティブ
        obj.SetActive(true);

        //弾の発射位置をPlayerの銃口に設定
        obj.transform.position = _shotPos.position;

        //弾の発射位置をPlayerの銃口に設定
        obj.transform.rotation = _shotPos.rotation;
    }


    //チェイン-----------------------------------------------------------------------------------------
    /// <summary>
    /// チェインの加算呼び出し
    /// </summary>
    public void ChainAddition()
    {

        //チェイン数の加算
        _chainCount++;

        //最大チェイン数の更新
        _scoreManager.ScoreUpdate(_chainCount);

        //加算後のチェイン数を表示
        _comboSystem.IncreaseCombo(_chainCount);

        //チェイン数に応じてコンボUIのサイズ変更
        _comboSystem.MathSize = _defChainFontSize + _chainCount;
        if (_comboSystem.MathSize >= _maxChainFontSize) { _comboSystem.MathSize = _maxChainFontSize; }

        //スキル量の加算
        if (_skillVol <= _maxSkillVol)
        {

            //チェイン加算量
            if (_chainCount < _chainMag.x) { _skillVol++; }
            else if (_chainMag.y > _chainCount && _chainCount >= _chainMag.x) { _skillVol += 2; }
            else if (_chainCount >= _chainMag.y) { _skillVol += 3; }
        }

        //最大値を超えていたら
        if (_skillVol > _maxSkillVol) { _skillVol = _maxSkillVol; }
    }

    /// <summary>
    /// チェインの初期化
    /// </summary>
    public void ChainResetting()
    {

        //コンボ数の初期化
        _comboSystem.RessetingCombo();

        //チェイン数の初期化
        _chainCount = 0;
    }


    //ロックオン-----------------------------------------------------------------------------------------
    /// <summary>
    /// 最大ロックオン数時にreturn
    /// </summary>
    public bool MaxLockCount() { if (_lockCount >= _maxLock) { return true; } return false; }

    /// <summary>
    /// ロックオン数を返却
    /// </summary>
    public int LockCount() { return _lockCount; }

    /// <summary>
    /// ロックオン(加算/減算/初期化)
    /// </summary>
    /// <param name="motion"></param>
    public void LockPrice(string motion)
    {

        if (motion == "加算") { if (_lockCount < 10) { _lockCount++; } }
        else if (motion == "減算") { if (_lockCount > 0) { _lockCount--; } }
        else if (motion == "初期化") { _lockCount = 0; }
    }

    /// <summary>
    /// ロックオン数に応じてレティクルの変更
    /// </summary>
    private void LockChange()
    {

        //ウルト中は処理しない
        if (_playerController.IsUlt()) { return; }

        //ロックオン数に応じたレティクルのサイズ
        if (_lockCount < _changeLock.x) { _pointerLockOn.ChangePointerClamp("1"); }
        else if (_changeLock.x <= _lockCount && _lockCount < _changeLock.y) { _pointerLockOn.ChangePointerClamp("2"); }
        else if (_changeLock.y <= _lockCount) { _pointerLockOn.ChangePointerClamp("3"); }
    }
}
