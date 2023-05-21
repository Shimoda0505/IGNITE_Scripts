using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// 画面内に描画されている敵はここに格納
/// 敵ごとのロックオン情報などが補完
/// </summary>
public class PlayerLinkEnemy : MonoBehaviour
{

    [Serializable, Tooltip("ターゲット関連の格納要素")]
    public class Targets
    {

        [SerializeField,Tooltip("格納されているターゲット")] public GameObject _targetObj;
        [SerializeField, Tooltip("ターゲットがロックオンされているかどうか")] public bool _isLock;
        [SerializeField, Tooltip("ロックオンのカーソル")] public GameObject _lockOnCursor;
        [SerializeField, Tooltip("ターゲットに追従する火球")] public GameObject _fireBoll;
        [SerializeField, Tooltip("配列に初めて入ったかどうか")] public bool _isInView;
    }
    [Header("ターゲットのList")]
    public  List<Targets> _targetList = new List<Targets>();


    [Header("カーソル")]
    [SerializeField, Tooltip("InViewカーソル")] private GameObject _inViewCursor;    
    private List<GameObject> _cursors = new List<GameObject>();//InViewCursor
    private bool _isBoss = false;//ボス戦開始中か



    //処理部--------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //カーソルを配列格納
        for (int i = 0; i <= _inViewCursor.transform.childCount - 1; i++)
        {

            //格納
            _cursors.Add(_inViewCursor.transform.GetChild(i).gameObject);

            //アクティブ終了
            _cursors[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {

        //ボス戦中は処理しない
        if (_isBoss) { return; }


        //Enemy配列内を全探索
        for (int i = 0; i <= _targetList.Count - 1; i++)
        {

            if(_targetList[i]._targetObj.tag == "Boss") { _isBoss = true; return; }

            //配列に初めて入った / Enemyタグ 
            if (!_targetList[i]._isInView && _targetList[i]._targetObj.tag == "Enemy")
            {

                //Cursor配列内を全探索
                for (int j = 0; j <= _cursors.Count - 1; j++)
                {

                    //使用してないものがあれば
                    if (!_cursors[j].activeSelf)
                    {

                        //カメラ描画中
                        _targetList[i]._isInView = true;

                        //ターゲットの設定
                        _cursors[j].GetComponent<InViewImage>()._target = _targetList[i]._targetObj;

                        //Active
                        _cursors[j].SetActive(true);

                        break;
                    }
                }
            }
        }
    }
}