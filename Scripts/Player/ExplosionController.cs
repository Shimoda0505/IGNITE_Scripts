using UnityEngine;



/// <summary>
/// �v���C���[�̒e���������鎞�̃G�t�F�N�g�����Ǘ�
/// </summary>
public class ExplosionController : MonoBehaviour
{

    public bool _isMove = default;//�ړ�����
    private float _count = default;//���Ԍv��

    [SerializeField,Tooltip("�A�N�e�B�u����")] private float _actFalseTime;



    //������------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //���Ԍv����A�N�e�B�u�I��
        _count += Time.deltaTime;
        if(_count >= _actFalseTime)
        {
            _count = 0;

            this.gameObject.SetActive(false);
        }
    }
}
