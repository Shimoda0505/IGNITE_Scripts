using UnityEngine;


/// <summary>
/// 敵(地面歩行関連)の挙動
/// </summary>
public class EnemyWalk : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("移動のルートナビ")] RootNav _rootNav;
    EnemyCameraView _enemyCameraView;//カメラビュー
    #endregion

    #region 位置
    [Header("位置")]
    [SerializeField, Tooltip("出現ポイント")] private GameObject _popPos;
    [SerializeField, Tooltip("始点")] private Transform _firstPos;
    [SerializeField, Tooltip("終点")] private Transform _endPos;
    #endregion

    #region 移動
    [Header("移動")]
    [SerializeField, Tooltip("移動速度")] private float _moveSpeed;
    [SerializeField, Tooltip("移動終了距離")] private float _endDistance;
    #endregion

    #region 時間
    [Header("時間")]
    [SerializeField, Tooltip("移動開始時間")] private float _startTime;
    [SerializeField, Tooltip("死んだあと消えるまでの時間")] private float _endTime;
    private float _count = 0;//時間計測
    #endregion

    private Animator _anim;//アニメーション
    Motion _motion = Motion.Wait;//行動enum
    enum Motion
    {
        Wait,
        Move,
        Death
    }



    //処理部----------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //カメラビュー
        _enemyCameraView = this.gameObject.GetComponent<EnemyCameraView>();

        //カメラビューfalse
        _enemyCameraView.enabled = false;

        //アニメーション
        _anim = this.gameObject.GetComponent<Animator>();

        //アニメーションfalse
        _anim.enabled = false;

        //移動開始位置に移動
        this.gameObject.transform.position = _firstPos.position;
    }


    private void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.Wait:

                //待機
                Wait();
                break;


            case Motion.Move:

                //移動
                Move();
                break;


            case Motion.Death:

                //死亡
                Death();
                break;
        }
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 待機
    /// </summary>
    private void Wait()
    {

        if (_rootNav.NowPoint() == _popPos)
        {

            //時間計測後に処理
            _count += Time.deltaTime;
            if (_count >= _startTime)
            {

                //時間初期化
                _count = 0;

                //アニメーションtrue
                _anim.enabled = true;

                //カメラビューtrue
                _enemyCameraView.enabled = true;

                //移動Enum
                _motion = Motion.Move;
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {

        //移動方向を向く
        this.gameObject.transform.LookAt(_endPos);

        //前方方向に移動
        this.gameObject.transform.position += this.gameObject.transform.forward * _moveSpeed;

        //終点との距離
        float _endPosDirection = (_endPos.position - transform.position).magnitude;

        //終点に近づいたらアクティブfalse
        if (_endPosDirection <= _endDistance) { this.gameObject.SetActive(false); }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Death()
    {
        //時間計測
        _count += Time.deltaTime;

        //計測後アクティブfalse
        if (_endTime <= _count) { this.gameObject.SetActive(false); }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void EnemyDeath()
    {

        //死亡アニメーション
        _anim.SetBool("Death", true);

        _motion = Motion.Death;
    }
}
