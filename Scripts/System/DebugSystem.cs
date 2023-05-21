using UnityEngine;


/// <summary>
/// デバック
/// </summary>
public class DebugSystem : MonoBehaviour
{


    [Header("Q/倍速")]
    [Header("W/等速")]
    [Header("A/ダメージ(100)")]
    [Header("C/カメラ揺れ")]
    [Header("D/死亡")]
    [Header("P/カメラview")]

    #region スクリプト
    [SerializeField, Tooltip("プレイヤーのステータス管理スクリプト")] PlayerStatus _playerStatus;
    [SerializeField, Tooltip("カメラ揺れスクリプト")] CameraShake _cameraShake;
    [SerializeField, Tooltip("プレイヤーの移動管理スクリプト")] PlayerController _playerController;
    [SerializeField, Tooltip("ゲームBGM管理スクリプト")] PlayerBgm _playerBgm;
    [SerializeField, Tooltip("スコア管理スクリプト")] ScoreManager _scoreManager;
    #endregion



    //処理部----------------------------------------------------------------------------------------------------------------
    private void Update()
    {

        //倍速
        if (Input.GetKeyDown(KeyCode.Q)) { Time.timeScale = 50; }
        //等速
        if (Input.GetKeyDown(KeyCode.W)) { Time.timeScale = 1; }
        //ダメージ
        if (Input.GetKeyDown(KeyCode.A)) { _playerStatus.Hit(100); }
        //カメラ揺れ
        if (Input.GetKeyDown(KeyCode.C)) { _cameraShake.Shake(3, 1); }
    }
}
