using UnityEngine;



/// <summary>
/// �Q�[���I�����̃J��������
/// </summary>
public class GameEndCam : MonoBehaviour
{

    [SerializeField, Tooltip("�C�x���g�p�J����")] private GameObject eventCamera;
    [SerializeField, Tooltip("�v���C���[")] private Transform _playerTr;
    [SerializeField, Tooltip("�ړ��Ǐ]�I�u�W�F�N�g")] private Transform _targetTr;
    private bool _isResult = false;

    private void LateUpdate()
    {

        if(_isResult)
        {
            //�J�����������v�Z���Đ��񑬓x���|�����
            eventCamera.transform.localPosition = _targetTr.localPosition;

            //�v���C���[�𒼎�
            eventCamera.transform.LookAt(_playerTr, Vector3.up);
        }
    }

    /// <summary>
    /// Result�J�����ɕύX
    /// </summary>
    public void ResultCam()
    {

        //�C�x���g�J�������A�N�e�B�u
        eventCamera.SetActive(true);

        //Result�J�n
        _isResult = true;
    }
}
