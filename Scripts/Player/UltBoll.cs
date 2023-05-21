using UnityEngine;



/// <summary>
/// �v���C���[�̃E���g���̉΋�����
/// </summary>
public class UltBoll : MonoBehaviour
{

    #region ����
    [Header("����")]
    [SerializeField, Tooltip("�ړ����x")] private float _moveSpeed;
    [SerializeField, Tooltip("�ړ�����")] private float _moveTime;
    [SerializeField, Tooltip("��������")] private float _exTime;
    [SerializeField, Tooltip("�ҋ@����")] private float _waitTime;
    private float _moveCount = 0;//�ړ����Ԃ̃J�E���g
    private GameObject _shutPos = default;//���ˈʒu
    private bool _isBoss = false;//�{�X�ɍU���������ǂ���
    private GameObject _bossObj = default;//�{�X�I�u�W�F�N�g
    private const int ATTACK_DAMAGE = 1000;//�U����
    #endregion


    #region �X�N���v�g
    PlayerLinkEnemy _playerLinkEnemy;//�G�Ǘ��X�N���v�g
    PlayerStatus _playerStatus;//�v���C���[�̃X�e�[�^�X�Ǘ��X�N���v�g
    BossStatus _bossStatus;//�{�X�̃X�e�[�^�X�Ǘ��X�N���v�g
    ScoreManager _scoreManager;//�X�R�A�Ǘ��X�N���v�g
    PoolController _exPool;//�I�u�W�F�N�g�v�[���̃X�N���v�g
    #endregion

    Motion _motion = Motion.WAIT;//�s��enum
    enum Motion
    {
        WAIT,
        MOVE,
        STOP
    }



    //������-------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�擾�֘A
        _playerLinkEnemy = GameObject.FindGameObjectWithTag("PlayerArray").GetComponent<PlayerLinkEnemy>();//�v���C���[�̉w�Ǘ��z��
        _playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();//�v���C���[�̃X�e�[�^�X
        _bossStatus = GameObject.FindGameObjectWithTag("BossBody").GetComponent<BossStatus>(); //�{�X�X�e�[�^�X
        _scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();//�X�R�A�Ǘ��X�N���v�g
        _exPool = GameObject.FindGameObjectWithTag("UltBollEx").GetComponent<PoolController>();//�I�u�W�F�N�g�v�[���̃X�N���v�g
        _shutPos = GameObject.FindGameObjectWithTag("ShutArea").gameObject;//���ˈʒu
    }


    void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.WAIT:

                //�e�̈ʒu�𔭎ˈʒu��
                this.gameObject.transform.position = _shutPos.transform.position;

                //���Ԍv���㏈��
                _moveCount += Time.deltaTime;
                if (_moveCount >= _waitTime)
                {

                    _moveCount = 0;
                    _motion = Motion.MOVE;
                }
                break;


            case Motion.MOVE:

                //�O�������ɒ��i
                this.gameObject.transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);

                //���Ԍv���㏈��
                _moveCount += Time.deltaTime;
                if (_moveCount >= _moveTime)
                {

                    //�I�u�W�F�N�g�v�[�����甚���G�t�F�N�g���Ăяo���ăA�N�e�B�u�J�n
                    GameObject obj = _exPool.GetObj();
                    obj.transform.position = this.gameObject.transform.position;
                    obj.SetActive(true);

                    //������
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSE>().ExplosionBigSe();

                    _moveCount = 0;
                    _motion = Motion.STOP;

                    //�G�Ƀ_���[�W
                    DamageEnemy();
                }
                break;


            case Motion.STOP:

                //���Ԍv���㏈��
                _moveCount += Time.deltaTime;
                if (_moveCount >= _exTime)
                {

                    //�e�̈ʒu��Ǐ]����Ώۂ�BurretPool�^�O�Ŏw��
                    this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerArray").transform.position;

                    //�A�N�e�B�u�I��
                    this.gameObject.SetActive(false);
                    _moveCount = 0;
                    _motion = Motion.WAIT;
                }
                break;
        }
    }



    //���\�b�h��-------------------------------------------------------------------------------------------------------------------------------------
    private void DamageEnemy()
    {

        //�G�i�[�̔z���S�T��
        for (int i = _playerLinkEnemy._targetList.Count - 1; i >= 0; i--)
        {

            //�G�Ǘ��z�񂩂�G���Q�Ƃ��Đݒ�
            PlayerLinkEnemy.Targets targets = _playerLinkEnemy._targetList[i];
            GameObject target = targets._targetObj;

            if (target.tag == "Enemy")
            {

                //�G�̃_���[�W���\�b�h
                if (target.GetComponent<EnemyDeath>()) { target.GetComponent<EnemyDeath>().EnemyDeathController(); }/*�y�������o�[�����삵�����ߓY�t���Ă܂���z*/

                //���j���̉��Z
                _scoreManager.SmashEnemyCount(1);
            }

            else if (target.tag == "Boss")
            {

                //�����N�̃��b�N�I����Ԃ�����
                if (targets._targetObj.GetComponent<EnemyCameraView>()) { targets._targetObj.GetComponent<EnemyCameraView>().IsLockFalse(); }

                //�{�X�I�u�W�F�N�g���i�[
                _bossObj = targets._targetObj;

                //�{�X����
                _isBoss = true;
            }

            //�΋��̃^�[�Q�b�g��Null��
            if (targets._fireBoll != null) { targets._fireBoll.GetComponent<FireBollController>().TargetNull(); }

            //�J�[�\���̃^�[�Q�b�g��Null��
            if (targets._lockOnCursor != null) { targets._lockOnCursor.GetComponent<CursorController>()._target = null; }

            //i�Ԃ̔z��v�f���폜
            _playerLinkEnemy._targetList.RemoveAt(i);

            //�`�F�C�����̉��Z
            _playerStatus.ChainAddition();
        }

        //�Ώۂ��{�X�Ȃ�
        if (_isBoss)
        {

            //�{�X�X�e�[�^�X�Ǘ��X�N���v�g���Q��
            BossStatus bossStatus = _bossObj.transform.root.gameObject.GetComponent<BossStatus>();

            //�_���[�W��^��
            //�E���g�_���[�W�̃A�j���[�V����������
            bossStatus.BossDamage(ATTACK_DAMAGE);
            bossStatus.IsUltTrue();

            _isBoss = false;
        }
    }
}
