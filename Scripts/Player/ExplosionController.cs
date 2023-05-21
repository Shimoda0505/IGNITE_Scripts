using UnityEngine;



/// <summary>
/// プレイヤーの弾が爆発する時のエフェクト挙動管理
/// </summary>
public class ExplosionController : MonoBehaviour
{

    public bool _isMove = default;//移動中か
    private float _count = default;//時間計測

    [SerializeField,Tooltip("アクティブ時間")] private float _actFalseTime;



    //処理部------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //時間計測後アクティブ終了
        _count += Time.deltaTime;
        if(_count >= _actFalseTime)
        {
            _count = 0;

            this.gameObject.SetActive(false);
        }
    }
}
