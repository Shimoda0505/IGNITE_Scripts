using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �v���C���[�̃X�L��UI���A�X�L���̎g�p�󋵂ɂ���ĕύX
/// </summary>
public class SkillUi : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�v���C���[�̋����Ǘ��X�N���v�g")] PlayerController _playerController;
    [SerializeField, Tooltip("�v���C���[�̃X�e�[�^�X�Ǘ��X�N���v�g")] PlayerStatus _playerStatus;
    #endregion


    #region �X�L��UI
    [Header("�X�L��Ui")]
    [SerializeField, Tooltip("�񕜉摜")] private Image _heal;
    [SerializeField, Tooltip("�V�[���h�摜")] private Image _shield;
    [SerializeField, Tooltip("�E���g�摜")] private Image _ult;
    private bool _isHeal = false;//�񕜎g�p�ł��邩
    private bool _isShield = false;//�V�[���h�g�p�ł��邩
    private bool _isUlt = false;//�E���g�g�p�ł��邩
    private float _maxSkillVol = default;//�X�L���̍ő�l
    #endregion



    //������----------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�X�L���̍ő�l
        _maxSkillVol = _playerStatus.MaxSkillVol();
    }

    private void FixedUpdate()
    {

        //���݂̃X�L����
        float skillVol = _playerStatus.SkillVol();

        //�X�L���̃C���^�[�o��
        float healPar = _playerController.SkillCount().x / _playerController.SkillTime().x;
        float shieldPar = _playerController.SkillCount().y / _playerController.SkillTime().y;
        float ultPar = _playerController.SkillCount().z / _playerController.SkillTime().z;

        //��
        if (healPar >= 1) { _isHeal = true; }
        else { _isHeal = false; }

        _heal.fillAmount = healPar;

        if (skillVol >= _maxSkillVol / 4 && _isHeal) { _heal.color = new Color(255, 255, 255, 1); }
        else { _heal.color = new Color(255, 255, 255, 0.2f); }


        //�V�[���h
        if (shieldPar >= 1) { _isShield = true; }
        else { _isShield = false; }

        _shield.fillAmount = shieldPar;

        if (skillVol >= _maxSkillVol / 2 && _isShield) { _shield.color = new Color(255, 255, 255, 1); }
        else { _shield.color = new Color(255, 255, 255, 0.2f); }


        //�E���g
        if (ultPar >= 1) { _isUlt = true; }
        else { _isUlt = false; }

        _ult.fillAmount = ultPar;

        if (skillVol >= _maxSkillVol && _isUlt) { _ult.color = new Color(255, 255, 255, 1); }
        else { _ult.color = new Color(255, 255, 255, 0.2f); }
    }
}
