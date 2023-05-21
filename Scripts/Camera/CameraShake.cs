using System.Collections;
using UnityEngine;


/// <summary>
/// �J������h�炷���߂̃X�N���v�g
/// </summary>
public class CameraShake : MonoBehaviour
{

    [SerializeField,Tooltip("�h�炷�Ώ�(��{�̓��C���J����)")] private Transform _moveObj;
    [SerializeField,Tooltip("�Q�[���C�x���g�Ǘ��X�N���v�g")] GameSystem _gameSystem;
    private bool _isShake;//�J�����D�h�ꒆ

    //�h�ꕝ
    private float xRange = default;//x���̗h�ꕝ
    private float yRange = default;//Y���̗h�ꕝ
    //���W
    private float shakeX = default;//x���̍��W
    private float shakeY = default;//y���̍��W


    //���\�b�h��------------------------------------------------------------------------------------------------
    /// <summary>
    /// �J�����̗h��
    /// </summary>
    /// <param name="time">�h�ꎞ��</param>
    /// <param name="magnitude">�h�ꕝ</param>
    public void Shake(float time, float magnitude)
    {
        if(!_isShake)
        {
            StartCoroutine(DoShake(time, magnitude));//���X�N���v�g����̈���

            _isShake = true;
        }
    }

    private IEnumerator DoShake(float time, float magnitude)
    {

        //�����l1
        xRange = 0;
        yRange = 0;

        var elapsed = 0f;//�^�C�}�[

        while (elapsed < time)//time���ԗh�炵������
        {

            if(_gameSystem.IsEvent())
            {
                _moveObj.localPosition = new Vector3(0, 0, _moveObj.localPosition.z);//x��y������L���ړ�

                //�����l1
                xRange = 0;
                yRange = 0;

                yield break;
            }

            //�㉺���E��Range*magunitude ���ړ�
            shakeX = Random.Range(-xRange, xRange) * magnitude;
            shakeY = Random.Range(-yRange, yRange) * magnitude;

            _moveObj.localPosition = new Vector3(shakeX, shakeY, _moveObj.localPosition.z);//x��y������L���ړ�

            elapsed += Time.deltaTime;//�^�C�}�[

            //�S�̂�1/3�b��,�h���傫�����Ă���
            if (elapsed < time / 2)
            {
                xRange += Time.deltaTime;
                yRange += Time.deltaTime;
            }

            //1/3�b��,�h������������Ă���
            else
            {
                xRange -= Time.deltaTime;
                if(xRange <= 0) { xRange = 0; }

                yRange -= Time.deltaTime;
                if(yRange <= 0) { yRange = 0; }
            }

            yield return null;

        }

        _isShake = false;
    }
}
