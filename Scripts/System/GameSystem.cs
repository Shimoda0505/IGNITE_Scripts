using UnityEngine;



/// <summary>
/// �Q�[���S�̂̃C�x���g�������Ǘ�
/// </summary>
public class GameSystem : MonoBehaviour
{

    //�C�x���g�Ǘ��t���O
    private bool _isEvent;



    //������-------------------------------------------------------------------------------------------------------
    //�J�[�\��������
    private void Start()
    {
        Cursor.visible = false;
    }


    //���\�b�h��----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Event�̏I��
    /// </summary>
    public void FalseIsEvent() { _isEvent = false; }

    /// <summary>
    /// Event�̊J�n
    /// </summary>
    public void TrueIsEvent() { _isEvent = true; }

    /// <summary>
    /// �C�x���g��Ԃ̕ԋp
    /// </summary>
    public bool IsEvent() { return _isEvent; }
}
