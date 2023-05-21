using UnityEngine;



/// <summary>
/// プレイヤーのエフェクトを自動でアクティブ終了
/// </summary>
public class PlayerEffectLooping : MonoBehaviour
{

    [SerializeField, Tooltip("消えるまでの時間")] private float _activeTime;
    private float _activeCount = 0;//時間の計測



    //処理部------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //時間計測後に処理
        _activeCount += Time.deltaTime;
        if(_activeCount >= _activeTime)
        {

            //時間の初期化
            _activeCount = 0;

            //アクティブ終了
            this.gameObject.SetActive(false);
        }
    }



    //メソッド部------------------------------------------------------------------------------------------
    /// <summary>
    /// カウントの初期化
    /// </summary>
    public void ResetCount() { _activeCount = 0; }
}
