using UnityEngine;



/// <summary>
/// プレイヤーの弾挙動を制御
/// </summary>
public class BulletController : MonoBehaviour
{
    #region 取得関連
    public GameObject _targetObj = default;//追従する対象
    PlayerStatus _playerStatus;//プレイヤーのステータス
    [SerializeField, Tooltip("初期位置")] private GameObject _defPos;
    #endregion


    #region 移動関連
    [SerializeField, Tooltip("弾速")] private float _moveSpeed;
    [SerializeField, Tooltip("旋回速度")] private float _rotateSpeed;
    #endregion


    #region 時間関連
    [SerializeField,Tooltip("消えるまでの時間")] private float _time = default;
    private float _count = 0;//時間計測
    #endregion

    #region 弾関連
    private const int ATTACK_DAMAGE = 100;//球の火力
    [SerializeField, Tooltip("弾の当たり判定範囲")] private float _hitClamp;
    #endregion



    //処理-------------------------------------------------------------------------------------------------
    void Awake()
    {

        //プレイヤーのステータスを取得
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    private void FixedUpdate()
    {
        if (_targetObj != null)
        {
            //弾の移動(追従)
            HomingMoveObj();

            //ターゲットとの距離を計測
            TargetDistance();

            //時間計測
            TimeCount();
        }
    }



    //publicのメソッド群-----------------------------------------------------------------------

    /// <summary>
    /// 弾の初期設定
    /// </summary>
    public void TargetStore(GameObject target) { _targetObj = target; }


    //privateのメソッド群-----------------------------------------------------------------------
    /// <summary>
    /// 弾の移動(追従)
    /// </summary>
    private void HomingMoveObj()
    {

        //ターゲットと弾の距離
        Vector3 targetDirection = _targetObj.transform.position - this.gameObject.transform.position;

        //始点,終点,速度 * 時間,振幅
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, targetDirection, _rotateSpeed * Time.deltaTime, 0f);

        //前方方向に直進
        this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

        //ターゲットの方向に向きを変える
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }


    /// <summary>
    /// ターゲットとの距離を計測とヒット検出
    /// </summary>
    private void TargetDistance()
    {

        //弾の位置
        Vector3 thisPos = this.gameObject.transform.position;

        //ターゲットの位置
        Vector3 enemyPos = _targetObj.transform.position;

        //次のポイントとの距離を計測
        float distans = Vector3.Distance(thisPos, enemyPos);

        //弾の範囲がターゲットの範囲内に入ったら
        if (-_hitClamp <= distans && distans <= _hitClamp)
        {

            //プレイヤーにダメージ
            _playerStatus.Hit(ATTACK_DAMAGE);

            //初期化
            Resetting();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Resetting()
    {

        _count = 0;

        //ターゲーット初期化
        _targetObj = null;

        //弾の位置を初期化
        this.gameObject.transform.position = default;

        //アクティブ終了
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 時間計測
    /// </summary>
    private void TimeCount()
    {

        //時間計測後に処理
        _count += Time.deltaTime;
        if (_count >= _time) { Resetting(); }
    }


    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(this.gameObject.transform.position, _hitClamp);
    }

}
