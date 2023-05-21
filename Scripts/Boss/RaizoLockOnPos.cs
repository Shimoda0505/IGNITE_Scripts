using UnityEngine;



/// <summary>
/// �{�X�̃��b�N�I�����ʂɃX�N���v�g���Z�b�g
/// </summary>
public class RaizoLockOnPos : MonoBehaviour
{

    [SerializeField,Tooltip("���b�N�I������")] private GameObject[] _lockPosObjs;


    private void Start()
    {
        
        //���b�N�I���|�C���g���ׂĂɃX�N���v�g������
        for(int i = 0; i <= _lockPosObjs.Length - 1; i++)
        {

            _lockPosObjs[i].AddComponent<EnemyCameraView>();
        }

        //�A�N�e�B�u�I��
        this.gameObject.GetComponent<RaizoLockOnPos>().enabled = false;
    }
}
