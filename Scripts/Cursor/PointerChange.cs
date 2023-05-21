using UnityEngine;



/// <summary>
/// ���b�N�I������E���g�g�p�ɉ����āA�J�[�\���̌����ڂ�ύX
/// </summary>
public class PointerChange : MonoBehaviour
{

    [SerializeField,Tooltip("���b�N�I���Ǘ��X�N���v�g")] PointerLockOn _pointerLockOn;
    [SerializeField,Tooltip("�ʏ탌�e�B�N��1")] private GameObject _nomalRet;
    [SerializeField,Tooltip("�ʏ탌�e�B�N��2")] private GameObject _nomalRet2;
    [SerializeField,Tooltip("�ʏ탌�e�B�N��3")] private GameObject _nomalRet3;
    [SerializeField,Tooltip("�E���g���e�B�N��")] private GameObject _ultRet;
    [SerializeField,Tooltip("�E���g���e�B�N���̃A�j���[�V����")] private Animator _ultRetAnim;
    [SerializeField,Tooltip("�E���g���e�B�N�����I������܂ł̎���")] private float _time;
    private float _count = 0;//���Ԍv��
    private bool _isUlt = false;//�E���g���e�B�N�����A�N�e�B�u����



    private void FixedUpdate()
    {
        
        //�E���g���e�B�N�����A�N�e�B�u�Ȃ珈��
        if(_isUlt)
        {

            //���Ԍv����ɏ���
            _count += Time.deltaTime;
            if(_count >= _time)
            {

                //���ԏ�����
                _count = 0;

                //�E���g�I��
                _isUlt = false;

                //�ʏ탌�e�B�N�����A�N�e�B�u
                _nomalRet.SetActive(true);

                //�E���g���e�B�N�����A�N�e�B�u
                _ultRet.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ���e�B�N���̕ύX
    /// </summary>
    public void ChangeRet()
    {

        //�m�[�}��
        if(_nomalRet.activeSelf)
        {

            //���e�B�N���̃A�N�e�B�u�ύX
            _nomalRet.SetActive(false);
            _ultRet.SetActive(true);

            //���b�N�I���͈͂̕ύX
            _pointerLockOn.ChangePointerClamp("�E���g");

            //�E���g���e�B�N���̃A�j���[�V����
            _ultRetAnim.SetTrigger("Move");

        }
        //�E���g
        else if (!_nomalRet.activeSelf)
        {

            //�E���g�J�n
            _isUlt = true;

            //�E���g���e�B�N���̃A�j���[�V����
            _ultRetAnim.SetTrigger("ReMove");
        }
    }


    /// <summary>
    /// �ʏ탌�e�B�N��1�Ԃ̂݃A�N�e�B�u
    /// </summary>
    public void Change1()
    {

        //�T�C�Y�ύX
        _nomalRet.transform.localScale = new Vector2(1, 1);

        //���e�B�N���̃A�N�e�B�u�ύX
        _nomalRet2.SetActive(false);
        _nomalRet3.SetActive(false);
    }

    /// <summary>
    /// �ʏ탌�e�B�N��2�Ԃ̂݃A�N�e�B�u
    /// </summary>
    public void Change2()
    {

        //�T�C�Y�ύX
        _nomalRet.transform.localScale = new Vector2(1.4f, 1.4f);

        //���e�B�N���̃A�N�e�B�u�ύX
        _nomalRet2.SetActive(true);
        _nomalRet3.SetActive(false);
    }

    /// <summary>
    /// �ʏ탌�e�B�N��3�Ԃ̂݃A�N�e�B�u
    /// </summary>
    public void Change3()
    {

        //�T�C�Y�ύX
        _nomalRet.transform.localScale = new Vector2(1.8f, 1.8f);

        //���e�B�N���̃A�N�e�B�u�ύX
        _nomalRet2.SetActive(true);
        _nomalRet3.SetActive(true);
    }

}
