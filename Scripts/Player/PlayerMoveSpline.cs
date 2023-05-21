using UnityEngine;
using UnityEngine.Splines;/*�ySpline��Editor�g�����K�v�ł��z*/


/// <summary>
/// �v���C���[��Spline��ړ��Ǘ�
/// </summary>
public class PlayerMoveSpline : MonoBehaviour
{

    #region �X�v���C���̃f�[�^
    [Header("�X�v���C��")]
    [SerializeField, Tooltip("�X�e�[�W�̃X�v���C��")] private SplineContainer _stageSpline;/*�ySpline��Editor�g�����K�v�ł��z*/
    [SerializeField, Tooltip("���[�v�̃X�v���C��")] private SplineContainer _loopSpline;/*�ySpline��Editor�g�����K�v�ł��z*/
    private SplineContainer _settingSpline;//�ݒ肳�ꂽ�X�v���C���@/*�ySpline��Editor�g�����K�v�ł��z*/
    private RootSpline _rootSpline = RootSpline.STAGE;//�ړ����̃X�v���C�����
    private enum RootSpline
    {

        STAGE,//�X�e�[�W
        LOOP,//���[�v
    }
    #endregion


    #region �ړ��֘A
    [Header("�ړ��֘A")]
    [SerializeField, Tooltip("���[�g���x")] private float _rootSpeed;
    [SerializeField, Tooltip("���x�̕ύX���x")] private float _changeingSpeed;
    private float _moveSpeed;//�ړ����x
    private float _changeSpeed;//�ύX��̑��x
    private float _defaultSpeed;//����x
    private Transform _moveTarget;//�X�v���C���ɉ����Ĉړ�������Ώ�
    private float _percentage;//��Ԃ̊���(0~1�̊Ԃ��n�_^�I�_�ňړ�)
    private Vector3 _prevPos;//�O�t���[���̃��[���h�ʒu
    private const int _endSpline = 1;//�X�v���C���̏I�_
    private const int _startSpline = 0;//�X�v���C���̊J�n    
    private bool _isStop = false;//��~��
    #endregion



    //������-----------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�X�e�[�W�̃X�v���C����ݒ�
        _settingSpline = _stageSpline;

        //�X�v���C���ɉ����Ĉړ�������Ώ�
        _moveTarget = this.gameObject.transform;

        //�ړ����x�����[�g���x�ɐݒ�
        _moveSpeed = _rootSpeed;
        _changeSpeed = _moveSpeed;
    }


    private void FixedUpdate()
    {

        //���������Ԃŉ��Z
        _percentage += Time.deltaTime * _moveSpeed;

        //�ړ����̃X�v���C�����
        switch (_rootSpline)
        {

            case RootSpline.STAGE:

                //�X�v���C���̃f�[�^�X�V
                UpdateSpline();
                break;


            case RootSpline.LOOP:

                //�X�v���C�������[�v
                LoopSpline();
                break;
        }

        //���x�̕ύX
        ChangeSpeed();

        //�x�W�F�Ȑ��ł̈ړ��Ɛi�s�����Ɋp�x��ύX
        PlayerMovePosRotate();
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------
    //�v���C�x�[�g���\�b�h--------------------------------------------------
    /// <summary>
    /// ���x�̕ύX
    /// </summary>
    private void ChangeSpeed()
    {

        if (_moveSpeed != _changeSpeed && !_isStop) { _moveSpeed = Mathf.MoveTowards(_moveSpeed, _changeSpeed, Time.deltaTime * _changeingSpeed); }
    }

    /// <summary>
    /// �X�v���C���̃f�[�^�X�V
    /// </summary>
    private void UpdateSpline()
    {

        //�I�_�ɂ����烋�[�v�̃X�v���C���ɕύX
        if (_percentage >= _endSpline)
        {

            //���[�v�̃X�v���C����ݒ�
            _settingSpline = _loopSpline;

            //��Ԃ̊�����������
            _percentage = _startSpline;

            //���[�v�ɑJ��
            _rootSpline = RootSpline.LOOP;
        }
    }

    /// <summary>
    /// �X�v���C�������[�v
    /// </summary>
    private void LoopSpline()
    {

        //�I�_�ɂ�����n�_�ɕύX,��Ԃ̊�����������
        if (_percentage >= _endSpline) { _percentage = _startSpline; }
    }

    /// <summary>
    /// �x�W�F�Ȑ��ł̈ړ��Ɛi�s�����Ɋp�x��ύX
    /// </summary>
    private void PlayerMovePosRotate()
    {

        // �v�Z�����ʒu�i���[���h���W�j���^�[�Q�b�g�ɑ��
        _moveTarget.position = _settingSpline.EvaluatePosition(_percentage);

        // ���݃t���[���̃t���[���ʒu
        Vector3 position = _moveTarget.position;

        // �ړ��ʂ��v�Z
        Vector3 moveVolume = position - _prevPos;

        // ����Update�Ŏg�����߂̑O�t���[���ʒu�⊮
        _prevPos = position;

        // �Î~���Ă����Ԃ��ƁA�i�s���������ł��Ȃ����߉�]���Ȃ�
        if (moveVolume == Vector3.zero) { return; }

        // �i�s�����Ɋp�x��ύX
        _moveTarget.rotation = Quaternion.LookRotation(moveVolume, Vector3.up);
    }


    //�p�u���b�N���\�b�h----------------------------------------------------
    /// <summary>
    /// �ړ����x�̕ύX(��~/�ړ�)
    /// </summary>
    public void ChangeMoveSpeed(string name)
    {

        if (name == "�ړ�")
        {

            //����x�ɕύX
            _moveSpeed = _defaultSpeed;

            _isStop = false;
        }
        else if (name == "��~")
        {

            //���x�̕ۊ�
            _defaultSpeed = _moveSpeed;

            //��~�ɕύX
            _moveSpeed = 0;

            _isStop = true;
        }
    }

    /// <summary>
    /// ���x�̕ύX
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeSplineSpeed(float speed) { _changeSpeed = speed; }
}
