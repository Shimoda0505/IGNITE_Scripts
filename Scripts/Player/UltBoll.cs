using UnityEngine;



/// <summary>
/// プレイヤーのウルト時の火球制御
/// </summary>
public class UltBoll : MonoBehaviour
{

    #region 挙動
    [Header("挙動")]
    [SerializeField, Tooltip("移動速度")] private float _moveSpeed;
    [SerializeField, Tooltip("移動時間")] private float _moveTime;
    [SerializeField, Tooltip("爆発時間")] private float _exTime;
    [SerializeField, Tooltip("待機時間")] private float _waitTime;
    private float _moveCount = 0;//移動時間のカウント
    private GameObject _shutPos = default;//発射位置
    private bool _isBoss = false;//ボスに攻撃したかどうか
    private GameObject _bossObj = default;//ボスオブジェクト
    private const int ATTACK_DAMAGE = 1000;//攻撃力
    #endregion


    #region スクリプト
    PlayerLinkEnemy _playerLinkEnemy;//敵管理スクリプト
    PlayerStatus _playerStatus;//プレイヤーのステータス管理スクリプト
    BossStatus _bossStatus;//ボスのステータス管理スクリプト
    ScoreManager _scoreManager;//スコア管理スクリプト
    PoolController _exPool;//オブジェクトプールのスクリプト
    #endregion

    Motion _motion = Motion.WAIT;//行動enum
    enum Motion
    {
        WAIT,
        MOVE,
        STOP
    }



    //処理部-------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //取得関連
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();//プレイヤーの駅管理配列
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();//プレイヤーのステータス
        _bossStatus = GameObject.FindGameObjectWithTag("BossBody").GetComponent<BossStatus>(); //ボスステータス
        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();//スコア管理スクリプト
        _exPool = GameObject.FindGameObjectWithTag("UltBollEx").GetComponent<PoolController>();//オブジェクトプールのスクリプト
        _shutPos = GameObject.FindGameObjectWithTag("ShutArea").gameObject;//発射位置
    }


    void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.WAIT:

                //弾の位置を発射位置に
                this.gameObject.transform.position = _shutPos.transform.position;

                //時間計測後処理
                _moveCount += Time.deltaTime;
                if (_moveCount >= _waitTime)
                {

                    _moveCount = 0;
                    _motion = Motion.MOVE;
                }
                break;


            case Motion.MOVE:

                //前方方向に直進
                this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

                //時間計測後処理
                _moveCount += Time.deltaTime;
                if (_moveCount >= _moveTime)
                {

                    //オブジェクトプールから爆発エフェクトを呼び出してアクティブ開始
                    GameObject obj = _exPool.GetObj();
                    obj.transform.position = this.gameObject.transform.position;
                    obj.SetActive(true);

                    //爆発音
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionBigSe();

                    _moveCount = 0;
                    _motion = Motion.STOP;

                    //敵にダメージ
                    DamageEnemy();
                }
                break;


            case Motion.STOP:

                //時間計測後処理
                _moveCount += Time.deltaTime;
                if (_moveCount >= _exTime)
                {

                    //弾の位置を追従する対象をBurretPoolタグで指定
                    this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

                    //アクティブ終了
                    this.gameObject.SetActive(false);
                    _moveCount = 0;
                    _motion = Motion.WAIT;
                }
                break;
        }
    }



    //メソッド部-------------------------------------------------------------------------------------------------------------------------------------
    private void DamageEnemy()
    {

        //敵格納の配列を全探索
        for (int i = _playerLinkEnemy._targetList.Count - 1; i >= 0; i--)
        {

            //敵管理配列から敵を参照して設定
            PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];
            GameObject target = targets._targetObj;

            if (target.tag == "Enemy")
            {

                //敵のダメージメソッド
                if (target.GetComponent<EnemyDeath>()) { target.GetComponent<EnemyDeath>().EnemyDeathController(); }/*【他メンバーが制作したため添付してません】*/

                //撃破数の加算
                _scoreManager.SmashEnemyCount(1);
            }

            else if (target.tag == "Boss")
            {

                //雷蔵君のロックオン状態を解除
                if (targets._targetObj.GetComponent<EnemyCameraView>()) { targets._targetObj.GetComponent<EnemyCameraView>().IsLockFalse(); }

                //ボスオブジェクトを格納
                _bossObj = targets._targetObj;

                //ボス判定
                _isBoss = true;
            }

            //火球のターゲットをNullに
            if (targets._fireBoll != null) { targets._fireBoll.GetComponent<FireBollController>().TargetNull(); }

            //カーソルのターゲットをNullに
            if (targets._lockOnCursor != null) { targets._lockOnCursor.GetComponent<CursorController>()._target = null; }

            //i番の配列要素を削除
            _playerLinkEnemy._targetList.RemoveAt(i);

            //チェイン数の加算
            _playerStatus.ChainAddition();
        }

        //対象がボスなら
        if (_isBoss)
        {

            //ボスステータス管理スクリプトを参照
            BossStatus bossStatus = _bossObj.transform.root.gameObject.GetComponent<BossStatus>();

            //ダメージを与え
            //ウルトダメージのアニメーションさせる
            bossStatus.BossDamage(ATTACK_DAMAGE);
            bossStatus.IsUltTrue();

            _isBoss = false;
        }
    }
}
