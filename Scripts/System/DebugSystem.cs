using UnityEngine;


/// <summary>
/// �f�o�b�N
/// </summary>
public class DebugSystem : MonoBehaviour
{


    [Header("Q/�{��")]
    [Header("W/����")]
    [Header("A/�_���[�W(100)")]
    [Header("C/�J�����h��")]
    [Header("D/���S")]
    [Header("P/�J����view")]

    #region �X�N���v�g
    [SerializeField, Tooltip("�v���C���[�̃X�e�[�^�X�Ǘ��X�N���v�g")] PlayerStatus _playerStatus;
    [SerializeField, Tooltip("�J�����h��X�N���v�g")] CameraShake _cameraShake;
    [SerializeField, Tooltip("�v���C���[�̈ړ��Ǘ��X�N���v�g")] PlayerController _playerController;
    [SerializeField, Tooltip("�Q�[��BGM�Ǘ��X�N���v�g")] PlayerBgm _playerBgm;
    [SerializeField, Tooltip("�X�R�A�Ǘ��X�N���v�g")] ScoreManager _scoreManager;
    #endregion



    //������----------------------------------------------------------------------------------------------------------------
    private void Update()
    {

        //�{��
        if (Input.GetKeyDown(KeyCode.Q)) { Time.timeScale = 50; }
        //����
        if (Input.GetKeyDown(KeyCode.W)) { Time.timeScale = 1; }
        //�_���[�W
        if (Input.GetKeyDown(KeyCode.A)) { _playerStatus.Hit(100); }
        //�J�����h��
        if (Input.GetKeyDown(KeyCode.C)) { _cameraShake.Shake(3, 1); }
    }
}
