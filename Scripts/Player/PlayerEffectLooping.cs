using UnityEngine;



/// <summary>
/// �v���C���[�̃G�t�F�N�g�������ŃA�N�e�B�u�I��
/// </summary>
public class PlayerEffectLooping : MonoBehaviour
{

    [SerializeField, Tooltip("������܂ł̎���")] private float _activeTime;
    private float _activeCount = 0;//���Ԃ̌v��



    //������------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //���Ԍv����ɏ���
        _activeCount += Time.deltaTime;
        if(_activeCount >= _activeTime)
        {

            //���Ԃ̏�����
            _activeCount = 0;

            //�A�N�e�B�u�I��
            this.gameObject.SetActive(false);
        }
    }



    //���\�b�h��------------------------------------------------------------------------------------------
    /// <summary>
    /// �J�E���g�̏�����
    /// </summary>
    public void ResetCount() { _activeCount = 0; }
}
