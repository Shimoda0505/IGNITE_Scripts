using UnityEngine;


/// <summary>
/// カメラ内に写ってるかの判別しplayerLinkEnemyスクリプトの配列に格納
/// </summary>
public class EnemyCameraView : MonoBehaviour
{

    //スクリプト
    PlayerLinkEnemy _playerLinkEnemy;//敵保管スクリプト
    PlayerStatus _playerStatus;//プレイヤーのステータス管理スクリプト
    GameSystem _gameSystem;//ゲームイベント管理スクリプト

    //その他
    private bool _isLock = false;//多重ロックオン避け
    private Rect rect = new Rect(0, 0, 1, 1); //スクリーン座標
    private const float _disMaxPos = 500;//カメラとの距離



    //処理-------------------------------------------------------------------------------------------------
    private void Start()
    {

        //プレイヤーのタグからPlayerStatusスクリプトを取得
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        //プレイヤー配列タグからPlayerLinkEnemyスクリプトを取得
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();

        _gameSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSystem>();
    }

    private void FixedUpdate()
    {

        if (_gameSystem.IsEvent()) { return; }

        //カメラの画面内にオブジェクトがあるかどうか
        CamView();
    }



    //メソッド群--------------------------------------------------------------------------------------------
    /// <summary>
    /// カメラの画面内にオブジェクトがあるかどうか
    /// </summary>
    private void CamView()
    {

        //オブジェクト座標の取得
        Transform targetPos = this.gameObject.transform;

        //前方方向にオブジェクトがあるかどうか
        Vector3 upperForward = Camera.main.WorldToScreenPoint(targetPos.position);

        //スクリーン座標を、カメラView座標(0~1,0~1)に変換
        Vector2 upperCam = Camera.main.WorldToViewportPoint(targetPos.position);

        //カメラの画角内なら && 前方方向にオブジェクトがあるか
        //プレイヤーの敵を格納したListに、このオブジェクトを格納
        if (rect.Contains(upperCam) && _disMaxPos >= upperForward.z && upperForward.z >= 0) { InPlayerArray(); }

        //カメラの画角外なら,自信を探索して配列要素を削除,弾のターゲットをNull,カーソルを削除
        else { OutPlayerArray(); }
    }

    /// <summary>
    /// プレイヤーの敵を格納したListに、このオブジェクトを格納
    /// </summary>
    public void InPlayerArray()
    {

        if (!_isLock)
        {

            //List内にオブジェクトとフラグを格納
            _playerLinkEnemy._targetList.Add(new PlayerLinkEnemy.Targets { _targetObj = this.gameObject, _isLock = false });

            //ロックオン開始
            _isLock = true;
        }
    }

    /// <summary>
    /// 自信を探索して配列要素を削除,弾のターゲットをNull,カーソルを削除
    /// </summary>
    public void OutPlayerArray()
    {

        for (int i = 0; i <= _playerLinkEnemy._targetList.Count - 1; i++)
        {

            //i番のListを設定
            PlayerLinkEnemy.Targets target = _playerLinkEnemy._targetList[i];

            //ターゲットなら
            if (target._targetObj == this.gameObject)
            {

                if (target._lockOnCursor != null)
                {

                    //カーソルを消す
                    target._lockOnCursor.GetComponent<CursorController>()._target = null;

                    //ロックオン数を減算
                    _playerStatus.LockPrice("減算");
                }

                //火球のターゲットをnull
                if (target._fireBoll != null) { target._fireBoll.GetComponent<FireBollController>().ChangeEnum(); }

                //i番の配列要素を削除
                _playerLinkEnemy._targetList.RemoveAt(i);

                break;
            }
        }

        //ロックオン終了
        _isLock = false;
    }

    /// <summary>
    /// ロックオン可能な状態に戻す
    /// </summary>
    public void IsLockFalse() { _isLock = false; }
}
