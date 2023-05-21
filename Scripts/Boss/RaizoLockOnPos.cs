using UnityEngine;



/// <summary>
/// ボスのロックオン部位にスクリプトをセット
/// </summary>
public class RaizoLockOnPos : MonoBehaviour
{

    [SerializeField,Tooltip("ロックオン部位")] private GameObject[] _lockPosObjs;


    private void Start()
    {
        
        //ロックオンポイントすべてにスクリプトをつける
        for(int i = 0; i <= _lockPosObjs.Length - 1; i++)
        {

            _lockPosObjs[i].AddComponent<EnemyCameraView>();
        }

        //アクティブ終了
        this.gameObject.GetComponent<RaizoLockOnPos>().enabled = false;
    }
}
