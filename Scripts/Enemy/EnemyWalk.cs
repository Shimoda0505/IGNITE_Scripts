using UnityEngine;


/// <summary>
/// �G(�n�ʕ��s�֘A)�̋���
/// </summary>
public class EnemyWalk : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�ړ��̃��[�g�i�r")] RootNav _rootNav;
    EnemyCameraView _enemyCameraView;//�J�����r���[
    #endregion

    #region �ʒu
    [Header("�ʒu")]
    [SerializeField, Tooltip("�o���|�C���g")] private GameObject _popPos;
    [SerializeField, Tooltip("�n�_")] private Transform _firstPos;
    [SerializeField, Tooltip("�I�_")] private Transform _endPos;
    #endregion

    #region �ړ�
    [Header("�ړ�")]
    [SerializeField, Tooltip("�ړ����x")] private float _moveSpeed;
    [SerializeField, Tooltip("�ړ��I������")] private float _endDistance;
    #endregion

    #region ����
    [Header("����")]
    [SerializeField, Tooltip("�ړ��J�n����")] private float _startTime;
    [SerializeField, Tooltip("���񂾂��Ə�����܂ł̎���")] private float _endTime;
    private float _count = 0;//���Ԍv��
    #endregion

    private Animator _anim;//�A�j���[�V����
    Motion _motion = Motion.Wait;//�s��enum
    enum Motion
    {
        Wait,
        Move,
        Death
    }



    //������----------------------------------------------------------------------------------------------------------------

    private void Start()
    {

        //�J�����r���[
        _enemyCameraView = this.gameObject.GetComponent<EnemyCameraView>();

        //�J�����r���[false
        _enemyCameraView.enabled = false;

        //�A�j���[�V����
        _anim = this.gameObject.GetComponent<Animator>();

        //�A�j���[�V����false
        _anim.enabled = false;

        //�ړ��J�n�ʒu�Ɉړ�
        this.gameObject.transform.position = _firstPos.position;
    }


    private void FixedUpdate()
    {

        switch (_motion)
        {

            case Motion.Wait:

                //�ҋ@
                Wait();
                break;


            case Motion.Move:

                //�ړ�
                Move();
                break;


            case Motion.Death:

                //���S
                Death();
                break;
        }
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �ҋ@
    /// </summary>
    private void Wait()
    {

        if (_rootNav.NowPoint() == _popPos)
        {

            //���Ԍv����ɏ���
            _count += Time.deltaTime;
            if (_count >= _startTime)
            {

                //���ԏ�����
                _count = 0;

                //�A�j���[�V����true
                _anim.enabled = true;

                //�J�����r���[true
                _enemyCameraView.enabled = true;

                //�ړ�Enum
                _motion = Motion.Move;
            }
        }
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    private void Move()
    {

        //�ړ�����������
        this.gameObject.transform.LookAt(_endPos);

        //�O�������Ɉړ�
        this.gameObject.transform.position += this.gameObject.transform.forward * _moveSpeed;

        //�I�_�Ƃ̋���
        float _endPosDirection = (_endPos.position - transform.position).magnitude;

        //�I�_�ɋ߂Â�����A�N�e�B�ufalse
        if (_endPosDirection <= _endDistance) { this.gameObject.SetActive(false); }
    }

    /// <summary>
    /// ���S
    /// </summary>
    private void Death()
    {
        //���Ԍv��
        _count += Time.deltaTime;

        //�v����A�N�e�B�ufalse
        if (_endTime <= _count) { this.gameObject.SetActive(false); }
    }

    /// <summary>
    /// ���S
    /// </summary>
    public void EnemyDeath()
    {

        //���S�A�j���[�V����
        _anim.SetBool("Death", true);

        _motion = Motion.Death;
    }
}
