using UnityEngine;



/// <summary>
/// �v���C���[�̃V�[���h�G�t�F�N�g���f�B�]���u
/// </summary>
public class ShildEffect : MonoBehaviour
{

    [SerializeField,Tooltip("�v���C���[�̃X�e�[�^�X�Ǘ��X�N���v�g")] PlayerStatus _playerStatus;
    [SerializeField,Tooltip("�V�[���h�G�t�F�N�g�̃}�e���A��")] Material _material;
    [SerializeField,Tooltip("�G�t�F�N�g�̃V�F�[�_�[id")] private int _dis = 0;



    //������------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�G�t�F�N�g�̃V�F�[�_�[id�擾
        _dis = Shader.PropertyToID("_Dissolve");
    }

    void FixedUpdate()
    {

        //�f�B�]���u�l��1�ȏ�Ȃ�V�[���h�G�t�F�N�g���A�N�e�B�u�I��
        float disVol = 1 - _playerStatus.ShildVolume();
        _material.SetFloat(_dis, disVol);
        if (disVol >= 1) { this.gameObject.SetActive(false); }
    }
}
