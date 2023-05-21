using UnityEngine;


/// <summary>
/// �v���C���[�̈�˂Ɛ�����Ǘ�
/// </summary>
public class PlayerController : MonoBehaviour
{

    #region public
    //�v���[���[�̏��
    public PlayerMotion _playerMotion = PlayerMotion.Fly;
    public enum PlayerMotion
    {
        Fly,//��s
        Dodge,//���
        Damage,//�_���[�W
        Death,//���S
        Revive,//����
    }
    public int _reverseInput = -1;//���͂̔��]�L�[�R���p
    /// <summary>
    /// �v���C���[�̍��E�ɓ����͈�
    /// </summary>
    public Vector3 MoveClamp() { return _moveClamp; }
    /// <summary>
    /// �v���C���[�����ۂɈړ�������
    /// </summary>
    public Vector3 InputMove() { return _inputMove; }
    /// <summary>
    /// �����̈ړ�����
    /// </summary>
    public float DoageDirection() { return _doageDirection; }
    /// <summary>
    /// �E���g�g�p����
    /// </summary>
    public bool IsUlt() { return _isUlt; }
    /// <summary>
    /// �X�L���g�p��̎��Ԍv��
    /// </summary>
    public Vector3 SkillCount() { return _skillCount; }
    /// <summary>
    /// �X�L���g�p��̃C���^�[�o��
    /// </summary>
    public Vector3 SkillTime() { return _skillTime; }
    /// <summary>
    /// ���G�t���O
    /// </summary>
    public bool IsInvincible() { return _isInvincible; }
    #endregion

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�v���C���[�̃A�j���[�V�����Ǘ�")] PlayerAnimator _playerAnimator;
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")] GameSystem _gameSystem;
    [SerializeField, Tooltip("�v���C���[�̃X�e�[�^�X")] PlayerStatus _playerStatus;
    [SerializeField, Tooltip("�v���C���[�X�v���C��")] PlayerMoveSpline _playerMoveSpline;
    [SerializeField, Tooltip("Cursor�ύX�X�N���v�g")] PointerChange _pointerChange;
    [SerializeField, Tooltip("�{�X�X�e�[�^�X�Ǘ��X�N���v�g")] BossStatus _bossStatus;
    [SerializeField, Tooltip("�v���C���[��se�Ǘ��X�N���v�g")] PlayerSE _playerSE;
    [SerializeField, Tooltip("�v���C���[�̃G�t�F�N�g�Ǘ��X�N���v�g")] PlayerEffect _playerEffect;
    _InputSystemController _inputController;//���͎���
    AnyUseMethod _anyUseMethod;//�悭�g�����\�b�h
    #endregion

    #region �X��
    [Header("�X��")]
    [SerializeField, Tooltip("�ړ��p�x�̑��x")] private Vector3 _rotateSpeed;
    [SerializeField, Tooltip("�ړ��p�x�߂莞�̑��x")] private Vector3 _reRotateSpeed;
    [SerializeField, Tooltip("�ړ��p�xClamp")] private Vector3 _rotateClamp;
    private Vector3 _inputRotate = default;//�{�̂̌X��
    #endregion

    #region �ړ�
    [Header("�ړ�")]
    [SerializeField, Tooltip("�ړ������̑��x")] private Vector2 _moveSpeed;
    [SerializeField, Tooltip("�ړ�����Clamp")] private Vector3 _moveClamp;
    private Vector3 _moveInput = default;//���X�e�B�b�N�̓���
    private Vector3 _inputMove = default;//�ړ����x
    #endregion

    #region ���
    [Header("���")]
    [SerializeField, Tooltip("����I���̎���")] private float _doageTime;
    [SerializeField, Tooltip("�����̈ړ���")] private float _doageMove;
    [SerializeField, Tooltip("����̈ړ����x")] private float _doageSpeed;
    private float _doageCount = 0;//����I���̎��Ԍv��
    private float _doageDirection = default;//�����̈ړ�����
    private float _doagePos = default;//���O�̈ʒu�Ɉړ��ʂ̉��Z(���ړ��ʒu)
    #endregion

    #region �X�L��
    [Header("�X�L��")]
    [SerializeField, Tooltip("�X�L���g�p��̃C���^�[�o��(��/�o���A/�E���g)")] private Vector3 _skillTime;
    private bool _isHealing = false;//�񕜂�����
    private bool _isBarrier = false; //�o���A�g�p������
    private bool _isUlt = false;//�E���g�g�p������
    private Vector3 _targetPos = default;//�E���g�g�p���̓G�ʒu
    private Vector3 _skillCount = default;//�X�L���g�p��̎��Ԍv��
    #endregion

    #region �_���[�W
    [Header("�_���[�W")]
    [SerializeField, Tooltip("�_���[�W��̃C���^�[�o��")] private float _damageTime;
    [SerializeField, Tooltip("�_���[�W��̖��G����")] private float _invincibleTime;    
    private float _damageCount = 0;//�_���[�W��̌v��
    private bool _isInvincible = false;//���G�t���O
    #endregion

    #region ���S�A����
    [Header("���S�A����")]
    [SerializeField, Tooltip("��������J�n�܂ł̎���")] private float _reviveTime;
    private float _reviveCount = default;//��������J�n�܂ł̎��Ԃ��v��
    #endregion



    //����-------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�X�L�����Ԃ̏�����
        _skillCount = _skillTime;
    }

    private void Update()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem.IsEvent()) { return; }

        switch (_playerMotion)
        {

            case PlayerMotion.Fly:

                //�ړ��̓���
                MoveInput();

                //����̓���
                DodgeInput();

                //�X�L���̓���
                SkillInput();
                break;


            case PlayerMotion.Death:

                break;
        }
    }


    private void FixedUpdate()
    {

        //����0��������,�C�x���g���Ȃ牽�����Ȃ�
        if (Time.timeScale == 0 || _gameSystem.IsEvent())
        {
            //�^�[�Q�b�g�𒼎�,Ult��
            if (_gameSystem.IsEvent() && _isUlt && _bossStatus._motion != BossStatus.Motion.END)
            {

                GameObject target = _playerStatus.UltTarget();

                if (target.tag == "Enemy") { this.gameObject.transform.LookAt(_targetPos); }
                else if (target.tag == "Boss") { this.gameObject.transform.LookAt(target.transform); }

            }
            //�Q�[���N���A
            else
            {

                //�{�̂�(X�̓��͒l,Y�̓��͒l,Z�̌X��)�ɉ�]
                this.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);

                //�H�΂����ƕ��s��s�̃A�j���[�V�����؂�ւ�
                _playerAnimator.ChangeFlyVerticalAnim();

                //���E�ړ����̃A�j���[�V��������l�ɖ߂�
                _playerAnimator.ReturnFlyHorizontalAnim();
            }

            return;
        }

        switch (_playerMotion)
        {

            //��s
            case PlayerMotion.Fly:

                //�H�΂����ƕ��s��s�̃A�j���[�V�����؂�ւ�
                _playerAnimator.ChangeFlyVerticalAnim();

                //�v���C���[�̈ړ��v�Z
                MoveProcess();

                //�v���[���[�̈ړ������s.���f
                MoveExecution();
                break;


            //���
            case PlayerMotion.Dodge:

                //�v���C���[�̉���ړ��̌v�Z
                DoageProcess();

                //�v���[���[�̈ړ������s.���f
                MoveExecution();
                break;


            //�_���[�W
            case PlayerMotion.Damage:

                //�_���[�W�̏���
                Damage();

                //�v���[���[�̈ړ������s.���f
                MoveExecution();
                break;


            //���S
            case PlayerMotion.Death:

                break;


            //����
            case PlayerMotion.Revive:

                //�����̏���
                Revive();
                break;
        }


        //���G����
        Invincible();

        //�X�L���g�p��̃C���^�[�o��
        ShillResetting();
    }



    //private���\�b�h�Q--------------------------------------------------------------------------------

    //����
    /// <summary>
    /// �ړ��̓���
    /// </summary>
    private void MoveInput()
    {

        //���X�e�B�b�N�̓��͂��Ă��鎞
        if (_inputController.LeftStick())
        {

            //���͗ʂ�0.1�𒴂��Ă���Ȃ�(�듮��h�~�p)
            if (Mathf.Abs(_inputController.LeftStickValue().x) >= 0.1f || Mathf.Abs(_inputController.LeftStickValue().y) >= 0.1f)
            {
                //���͂Ɏ΂߂̓��͂𐳋K��
                _moveInput = _anyUseMethod.InputNomarizeVector2(_inputController.LeftStickValue(), _reverseInput * -1);

                //���E�ړ����̃A�j���[�V�����؂�ւ�
                _playerAnimator.ChangeFlyHorizontalAnim(_moveInput.x);
            }
        }

        //���X�e�B�b�N�̓��͂��Ă��Ȃ���
        else if (!_inputController.LeftStick())
        {

            //���E�ړ����̃A�j���[�V��������l�ɖ߂�
            _playerAnimator.ReturnFlyHorizontalAnim();
        }
    }

    /// <summary>
    /// �X�L���g�p��̃C���^�[�o��
    /// </summary>
    private void ShillResetting()
    {

        //��
        if (_isHealing)
        {

            //���Ԍv�����false
            _skillCount.x += Time.deltaTime;
            if (_skillCount.x >= _skillTime.x) { _isHealing = false; }
        }

        //�o���A
        if (_isBarrier)
        {

            //���Ԍv����o���Afalse
            _skillCount.y += Time.deltaTime;
            if (_skillCount.y >= _skillTime.y) { _isBarrier = false; }
        }

        //�E���g
        if (_isUlt)
        {

            //���Ԍv����
            _skillCount.z += Time.deltaTime;
            if (_skillCount.z >= _skillTime.z)
            {

                //�E���gfalse
                _isUlt = false;

                //���e�B�N���̕ύX
                _pointerChange.ChangeRet();
            }
        }
    }

    /// <summary>
    /// �X�L���̎g�p
    /// </summary>
    private void SkillInput()
    {

        //X�{�^���̓���
        if (_inputController.ButtonWestDown() && !_isHealing)
        {

            //�񕜂̎g�p�ƃX�L���g�p�t���Otrue
            _isHealing = _playerStatus.UseSkill("��");
            if (_isHealing)
            {

                //��SE
                _playerSE.HealSe();

                //���ԏ�����
                _skillCount.x = 0;
            }
        }

        //B�{�^���̓���
        else if (_inputController.ButtonEastDown() && !_isBarrier)
        {

            //�o���A�̎g�p�ƃX�L���g�p�t���Otrue
            _isBarrier = _playerStatus.UseSkill("�o���A");

            if (_isBarrier) { _skillCount.y = 0; }
        }

        //Y�{�^���̓���
        else if (_inputController.ButtonNorthDown() && !_isUlt)
        {

            //�E���g�̎g�p�ƃX�L���g�p�t���Otrue
            _isUlt = _playerStatus.UseSkill("�E���g");

            if (_isUlt)
            {

                //���ԏ�����
                _skillCount.z = 0;

                //�^�[�Q�b�g��Null���ǂ���
                if (_playerStatus.UltTarget() != null) { _targetPos = _playerStatus.UltTarget().transform.position; }
            }
        }
    }


    //��s
    /// <summary>
    /// �v���C���[�̈ړ��v�Z
    /// </summary>
    private void MoveProcess()
    {

        //���X�e�B�b�N�̓���
        if (_inputController.LeftStick())
        {

            //�p�x����
            //���͂ɐ��������Ċp�x���Z
            _inputRotate = _anyUseMethod.MoveClampVector3(_inputRotate, new Vector3(-_moveInput.y, _moveInput.x, -_moveInput.x), _rotateSpeed, -_rotateClamp, _rotateClamp);

            //�ړ�����
            //���͂ɐ��������Ĉړ�
            _inputMove = _anyUseMethod.MoveClampVector3(_inputMove, _moveInput, _moveSpeed, -_moveClamp, _moveClamp);
        }
        //���X�e�B�b�N�̓��͂Ȃ�
        else if (!_inputController.LeftStick())
        {

            //���͒l�̏�����
            _moveInput = new Vector2(0, 0);

            //���͒l�������l�ɖ߂�(�n�_,�I�_,���x)
            _inputRotate = _anyUseMethod.MoveToWardsAngleVector3(_inputRotate, new Vector3(0, 0, 0), Time.deltaTime * _reRotateSpeed);
        }
    }

    /// <summary>
    /// �v���[���[�̈ړ������s.���f
    /// </summary>
    private void MoveExecution()
    {

        //�{�̂̈ʒu�ړ�
        this.gameObject.transform.localPosition = new Vector3(_inputMove.x, _inputMove.y, 0);

        //�{�̂�(X�̓��͒l,Y�̓��͒l,Z�̌X��)�ɉ�]
        this.gameObject.transform.localEulerAngles = new Vector3(_inputRotate.x, _inputRotate.y, _inputRotate.z);
    }


    //���
    /// <summary>
    /// ����̓���
    /// </summary>
    private void DodgeInput()
    {

        //A�{�^���������Ɖ��
        if (_inputController.ButtonSouthDown())
        {

            //����A�j���[�V�����̍Đ��Ɖ������̕ԋp
            //�Ō�ɓ��͂��������ɉ��
            _doageDirection = _playerAnimator.DodgeAnim(_inputController.LeftStickValue().x);

            //���O�̈ʒu�Ɉړ��ʂ����Z(���ړ��ʒu)
            _doagePos = _inputMove.x + _doageMove * _doageDirection;

            //���SE
            _playerSE.BrinkSe();

            //����G�t�F�N�g
            _playerEffect.Doage();

            //���Enum�ɑJ��
            _playerMotion = PlayerMotion.Dodge;
        }
    }

    /// <summary>
    /// �v���C���[�̉���ړ��̌v�Z
    /// </summary>
    private void DoageProcess()
    {

        //�����̈ړ�
        _inputMove.x = _anyUseMethod.LerpClampVector3(_inputMove, new Vector3(_doagePos, 0, 0), new Vector3(_doageSpeed, 0, 0), -_moveClamp, _moveClamp).x;

        //���Ԃ̌v��
        _doageCount += Time.deltaTime;

        //���Ԃ���������Fly(Enum)�ɖ߂�
        if (_doageCount >= _doageTime)
        {

            //���ԏ�����
            _doageCount = 0;

            //��sEnum�ɑJ��
            _playerMotion = PlayerMotion.Fly;
        }
    }


    //�_���[�W
    /// <summary>
    /// �_���[�W�̏���
    /// </summary>
    private void Damage()
    {

        //���Ԃ̌v��
        _damageCount += Time.deltaTime;

        if (_damageCount >= _damageTime)
        {

            //�_���[�W�̎��Ԃ�������
            _damageCount = 0;

            //�_���[�W�A�j���[�V�������~
            _playerAnimator.DamageFalse();

            //���G�t���O
            _isInvincible = true;

            //��sEnum�ɑJ��
            _playerMotion = PlayerMotion.Fly;
        }
    }

    /// <summary>
    /// ���G����
    /// </summary>
    private void Invincible()
    {

        if (_isInvincible)
        {

            //���Ԃ̌v��
            _damageCount += Time.deltaTime;
            if (_damageCount >= _invincibleTime)
            {

                //�_���[�W�̎��Ԃ�������
                _damageCount = 0;

                //���G�t���O
                _isInvincible = false;
            }
        }
    }


    //���S
    /// <summary>
    /// �����̏���
    /// </summary>
    private void Revive()
    {

        //���Ԍv��
        _reviveCount += Time.deltaTime;

        //���Ԍo�߂�����
        if (_reviveCount >= _reviveTime)
        {

            //���Ԃ�������
            _reviveCount = 0;

            //�ړ��J�n
            _playerMoveSpline.ChangeMoveSpeed("�ړ�");

            //�ړ�Enum�ɑJ��
            _playerMotion = PlayerMotion.Fly;
        }
    }


    //public���\�b�h�Q--------------------------------------------------------------------------------

    /// <summary>
    /// ���S�J�n�̏���
    /// </summary>
    public void DeathStart()
    {

        //�ړ���~
        _playerMoveSpline.ChangeMoveSpeed("��~");

        //���S�A�j���[�V�����Đ�
        _playerAnimator.DeathAnim();

        //�v���C���[��Enum�����S�ɑJ��
        _playerMotion = PlayerMotion.Death;
    }
}