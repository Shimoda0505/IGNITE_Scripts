using UnityEngine;



/// <summary>
/// プレイヤーのシールドエフェクトをディゾルブ
/// </summary>
public class ShildEffect : MonoBehaviour
{

    [SerializeField,Tooltip("プレイヤーのステータス管理スクリプト")] PlayerStatus _playerStatus;
    [SerializeField,Tooltip("シールドエフェクトのマテリアル")] Material _material;
    [SerializeField,Tooltip("エフェクトのシェーダーid")] private int _dis = 0;



    //処理部------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //エフェクトのシェーダーid取得
        _dis = Shader.PropertyToID("_Dissolve");
    }

    void FixedUpdate()
    {

        //ディゾルブ値が1以上ならシールドエフェクトをアクティブ終了
        float disVol = 1 - _playerStatus.ShildVolume();
        _material.SetFloat(_dis, disVol);
        if (disVol >= 1) { this.gameObject.SetActive(false); }
    }
}
