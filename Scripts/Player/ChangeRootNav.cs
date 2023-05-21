using UnityEngine;



/// <summary>
/// �v���C���[���ړ����郋�[�g��ύX
/// </summary>
public class ChangeRootNav : MonoBehaviour
{

    [SerializeField, Tooltip("�v���C���[�̃��[�g�ړ��Ǘ��X�N���v�g")] RootNav _rootNav;
    [SerializeField,Tooltip("�R�I�u�W�F�N�g")] private GameObject _mountainObj;
    [SerializeField,Tooltip("�X�V���W")] private GameObject _updatePos;



    //������--------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {

        //���̃X�v���C���ɍX�V
        if (_updatePos == _rootNav.NowPoint())
        {

            //�l���A�N�e�B�u
            _mountainObj.SetActive(true);

            //�X�N���v�g�I��
            this.gameObject.GetComponent<ChangeRootNav>().enabled = false;
        }
    }
}
