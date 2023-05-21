using UnityEngine;


/// <summary>
/// �{�X�펞�̃J�b�g�C���Ǘ�
/// </summary>
public class CutInStart : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�v���C���[�̃��[�g�C�x���g�Ǘ��X�N���v�g")] RootNav _rootNav;
    [SerializeField, Tooltip("�J�b�g�C���Ǘ��X�N���v�g")] Cutin _cutin;/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/
    [SerializeField, Tooltip("�v���C���[��Spline�ړ��Ǘ��X�N���v�g")] PlayerMoveSpline _playerMoveSpline;
    [SerializeField, Tooltip("�Q�[���C�x���g�Ǘ��X�N���v�g")] GameSystem _gameSystem;
    [SerializeField, Tooltip("�{�X�X�e�[�^�X�Ǘ��X�N���v�g")] BossStatus _bossStatus;
    [SerializeField, Tooltip("�Q�[��BGM�Ǘ��X�N���v�g")] PlayerBgm _playerBgm;
    #endregion


    #region ���̑�
    [Header("���̑�")]
    [SerializeField, Tooltip("�J�n�ʒu")] private GameObject _startPosObj;
    [SerializeField, Tooltip("�ړ��J�n�܂ł̎���")] private float _time;
    private float _count = 0;//�J�b�g�C�����Ԃ̃J�E���g
    #endregion


     Motion _motion = Motion.WAIT;//�J�b�g�C��enum
    enum Motion
    {
        WAIT,
        EVENT
    }



    //������-----------------------------------------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        switch (_motion)
        {

            case Motion.WAIT:

                if (_rootNav.NowPoint() == _startPosObj)
                {

                    //�ړ���~
                    _playerMoveSpline.ChangeMoveSpeed("��~");

                    //�C�x���g�J�n
                    _gameSystem.TrueIsEvent();

                    //BGM�ύX
                    _playerBgm.SecondBgm();

                    //�J�b�g�C���̊J�n
                    _cutin.RaizoCutIn();

                    _motion = Motion.EVENT;
                }
                break;


            case Motion.EVENT:

                //���Ԍo�ߌ�ړ�
                _count += Time.deltaTime;
                if(_count >= _time) { _playerMoveSpline.ChangeMoveSpeed("�ړ�"); }

                //�J�b�g�C�����Ȃ�
                if(_cutin.IsCutIn())
                {

                    //������
                    _count = 0;

                    //�{�X�퓬�J�n
                    _bossStatus.IsStart();

                    //�C�x���g�I��
                    _gameSystem.FalseIsEvent();

                    //�A�N�e�B�u�I��
                    this.gameObject.SetActive(false);
                }

                break;
        }
    }
}
