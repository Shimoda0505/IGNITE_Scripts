using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �Q�[���J�n���o�̊Ǘ�
/// </summary>
public class GameStart : MonoBehaviour
{

    #region �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField, Tooltip("�Q�[���̃C�x���g��Time.deltaTime���Ǘ�")] GameSystem _gameSystem;
    [SerializeField, Tooltip("�Q�[����BGM")] PlayerBgm _gameBgm;
    #endregion


    #region ���̑�
    [Header("���̑�")]
    [SerializeField, Tooltip("���e�B�N��")] private GameObject _reticle;
    [SerializeField, Tooltip("�t�F�[�h�A�E�g�p�摜")] private Image FadeImage;
    [SerializeField, Tooltip("�C�x���g�p�J����")] private GameObject _eventCam;
    [SerializeField, Tooltip("���C���J����")] private GameObject _mainCam;
    #endregion



    //������---------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�C�x���g�J�����A�N�e�B�u
        _eventCam.SetActive(true);

        //���C���J�����̃A�N�e�B�u�I��
        _mainCam.GetComponent<Camera>().enabled = false;

        //�C�x���g�̊J�n
        _gameSystem.TrueIsEvent();

        //1�Ԃ�Bgm�J�n
        _gameBgm.OneBgm();

        //���e�B�N������
        _reticle.SetActive(false);

        //�t�F�[�h�C��
        StartCoroutine("FadeIn");
    }



    //���\�b�h��---------------------------------------------------------------------------------------------------------------------------
    public void StartCamEnd()
    {

        //�C�x���g�̏I��
        _gameSystem.FalseIsEvent();

        //���C���J�����̃A�N�e�B�u
        _mainCam.GetComponent<Camera>().enabled = true;

        //�J�[�\���̃A�N�e�B�u
        _reticle.SetActive(true);
    }

    //�t�F�[�h�C��
    IEnumerator FadeIn()
    {

        //�t�F�[�h�C��
        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1);

        //�t�F�[�h�A�E�g
        for (float i = 1; i > 0; i -= 0.01f)
        {

            FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, i);
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
}
