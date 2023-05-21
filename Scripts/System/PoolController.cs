using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �I�u�W�F�N�g�v�[��
/// </summary>
public class PoolController : MonoBehaviour
{

    public static PoolController instans;//����̃I�u�W�F�N�g�ɂ���A�X�N���v�g���Q��
    public List<GameObject> _poolObjList;//���������e�p�̃��X�g

    [SerializeField, Tooltip("�v�[���ɐ����������e����")] private GameObject _poolObj;
    [SerializeField, Tooltip("�ŏ��ɐ�������e�̍ő吔 ")] private int _maxPool;



    //������-------------------------------------------------------------------------------------------------
    void Awake()
    {

        //�ŏ��ɌŒ萔�v���n�u�I�u�W�F�N�g���C���X�^���g
        InitializeObj();
    }



    //���\�b�h��--------------------------------------------------------------------------------------------
    /// <summary>
    /// �ŏ��ɌŒ萔�v���n�u�I�u�W�F�N�g���C���X�^���g
    /// </summary>
    private void InitializeObj()
    {

        // �ŏ��ɂ�����x�̐��A�I�u�W�F�N�g���쐬���ăv�[���ɗ��߂Ă�������
        //�e�̐���������
        _poolObjList = new List<GameObject>();

        //�e���ő吔�܂Ő���
        for (int i = 0; i < _maxPool; i++)
        {

            // �e�𐶐�����
            GameObject newObj = CreateNewObj();

            //���������e�̕���������false
            newObj.SetActive(false);

            // ���X�g�ɕۑ����Ă���
            _poolObjList.Add(newObj);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[������g�p����v���n�u�I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>�v���n�u�I�u�W�F�N�g</returns>
    public GameObject GetObj()
    {
        // ���g�p������Ύg�p,�Ȃ���ΐ���
        // �g�p���łȂ����̂�T���ĕԂ�
        //���X�g���ɂ���e��obj�Ƃ��ĕԂ�
        foreach (var obj in _poolObjList)
        {
            //�e��Rigidbody���擾
            bool objrb = obj.activeSelf;

            //�e�̕���������false�̂��̂�T��
            if (objrb == false)
            {

                //�e�̕���������false�̂��̂�����΂����true
                objrb = true;

                //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
                return obj;
            }
        }

        // �S�Ďg�p����������V�������
        GameObject newObj = CreateNewObj();

        //���X�g�ɕۑ����Ă���
        _poolObjList.Add(newObj);

        //�V����������I�u�W�F�N�g�̕������������̂܂�true�ɂ���
        newObj.SetActive(false);

        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }

    /// <summary>
    /// �V�����v���n�u�I�u�W�F�N�g���C���X�^���g
    /// </summary>
    /// <returns>�V�����v���n�u�I�u�W�F�N�g</returns>
    private GameObject CreateNewObj()
    {

        // ��ʊO�ɐ���
        Vector3 pos = this.gameObject.transform.position;

        // �V�����e�𐶐�(�����������e��,��ʊO��,�e�ɂȂ�I�u�W�F�N�g�Ɠ�������)
        GameObject newObj = Instantiate(_poolObj, pos, Quaternion.identity);

        // ���O�ɘA�ԕt��(���X�g�ɒǉ����ꂽ��)
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }
}
