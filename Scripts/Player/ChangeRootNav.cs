using UnityEngine;



/// <summary>
/// プレイヤーが移動するルートを変更
/// </summary>
public class ChangeRootNav : MonoBehaviour
{

    [SerializeField, Tooltip("プレイヤーのルート移動管理スクリプト")] RootNav _rootNav;
    [SerializeField,Tooltip("山オブジェクト")] private GameObject _mountainObj;
    [SerializeField,Tooltip("更新座標")] private GameObject _updatePos;



    //処理部--------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //次のスプラインに更新
        if (_updatePos == _rootNav.NowPoint())
        {

            //様をアクティブ
            _mountainObj.SetActive(true);

            //スクリプト終了
            this.gameObject.GetComponent<ChangeRootNav>().enabled = false;
        }
    }
}
