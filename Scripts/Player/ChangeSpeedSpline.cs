using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// スプライン上の移動速度を変更
/// </summary>
public class ChangeSpeedSpline : MonoBehaviour
{

    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーの現在位置を補完し、各種イベントを発生させるスクリプト")] RootNav _rootNav;
    [SerializeField, Tooltip("プレイヤーのルート移動管理スクリプト")] PlayerMoveSpline _playerMoveSpline;
    
    [Serializable, Tooltip("行動データ")]
    private class ActionDatas
    {

        [SerializeField,Tooltip("変更位置")]
        public GameObject _changePos;

        [SerializeField,Tooltip("変更速度")]
        public float _speed;
    }
    [SerializeField, Tooltip("行動データ")] private List<ActionDatas> _actionDatas = new List<ActionDatas>();
    private int _listNumber = 0;//現在の配列番号



    //処理部---------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        if(_listNumber <= _actionDatas.Count - 1)
        {

            //範囲内に入ったら
            if (_rootNav.NowPoint() == _actionDatas[_listNumber]._changePos)
            {

                //速度の変更
                _playerMoveSpline.ChangeSplineSpeed(_actionDatas[_listNumber]._speed);

                //リストの更新
                _listNumber++;
            }
        }
    }
}
