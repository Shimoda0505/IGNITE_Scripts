using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーのスキルUIを、スキルの使用状況によって変更
/// </summary>
public class SkillUi : MonoBehaviour
{

    #region スクリプト
    [Header("スクリプト")]
    [SerializeField, Tooltip("プレイヤーの挙動管理スクリプト")] PlayerController _playerController;
    [SerializeField, Tooltip("プレイヤーのステータス管理スクリプト")] PlayerStatus _playerStatus;
    #endregion


    #region スキルUI
    [Header("スキルUi")]
    [SerializeField, Tooltip("回復画像")] private Image _heal;
    [SerializeField, Tooltip("シールド画像")] private Image _shield;
    [SerializeField, Tooltip("ウルト画像")] private Image _ult;
    private bool _isHeal = false;//回復使用できるか
    private bool _isShield = false;//シールド使用できるか
    private bool _isUlt = false;//ウルト使用できるか
    private float _maxSkillVol = default;//スキルの最大値
    #endregion



    //処理部----------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //スキルの最大値
        _maxSkillVol = _playerStatus.MaxSkillVol();
    }

    private void FixedUpdate()
    {

        //現在のスキル量
        float skillVol = _playerStatus.SkillVol();

        //スキルのインターバル
        float healPar = _playerController.SkillCount().x / _playerController.SkillTime().x;
        float shieldPar = _playerController.SkillCount().y / _playerController.SkillTime().y;
        float ultPar = _playerController.SkillCount().z / _playerController.SkillTime().z;

        //回復
        if (healPar >= 1) { _isHeal = true; }
        else { _isHeal = false; }

        _heal.fillAmount = healPar;

        if (skillVol >= _maxSkillVol / 4 && _isHeal) { _heal.color = new Color(255, 255, 255, 1); }
        else { _heal.color = new Color(255, 255, 255, 0.2f); }


        //シールド
        if (shieldPar >= 1) { _isShield = true; }
        else { _isShield = false; }

        _shield.fillAmount = shieldPar;

        if (skillVol >= _maxSkillVol / 2 && _isShield) { _shield.color = new Color(255, 255, 255, 1); }
        else { _shield.color = new Color(255, 255, 255, 0.2f); }


        //ウルト
        if (ultPar >= 1) { _isUlt = true; }
        else { _isUlt = false; }

        _ult.fillAmount = ultPar;

        if (skillVol >= _maxSkillVol && _isUlt) { _ult.color = new Color(255, 255, 255, 1); }
        else { _ult.color = new Color(255, 255, 255, 0.2f); }
    }
}
